using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 用户触摸事件处理器
    // 这里才是负责处理触摸的地方。
    public class UserController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    UpdateTouch(touch);
                }
            }
        }
        private void UpdateTouch(Touch touch)
        {
            switch (touch.phase)
            {
                // 单点
                case TouchPhase.Began:
                    Tap(touch);
                    break;
                // 移动与长按
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Hold(touch);
                    break;
                // 释放
                case TouchPhase.Ended:
                    Release(touch);
                    break;
            }
        }


        // 根据touch获取RaycastHit2D
        private RaycastHit2D GetTouch(Touch touch)
        {
            Ray FRay = Camera.main.ScreenPointToRay(touch.position);
            Vector3 touchPos = new Vector3(FRay.origin.x, FRay.origin.y, -10);
            
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector3.back, Mathf.Infinity, 1 << LayerMask.NameToLayer("Notes"));

            return hit;
        }

        // 根据ray获取Note
        private Note GetNote(RaycastHit2D ray)
        {
            if (!ray) return null;

            return ray.collider.gameObject.GetComponent<NoteController>().Note;
        }


        // 处理单点
        private void Tap(Touch touch)
        {
            Note note = GetNote(GetTouch(touch));

            if (note == null) return;

            note.Click();
        }

        // 处理移动和长按
        private void Hold(Touch touch)
        {
            RaycastHit2D hit = GetTouch(touch);

            if (!hit) 
            {
                return;
            }

            Note note = GetNote(hit);
            
            if (note == null) return;
            
            note.Hold();
        }

        // 处理释放
        private void Release(Touch touch)
        {
            Note note = GetNote(GetTouch(touch));

            if (note == null) return;

            note.Release();
        }
    }
}