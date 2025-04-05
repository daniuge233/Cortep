using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace HomePage
{
    public class UserController : MonoBehaviour
    {
        public bool isActive = true;

        public GameObject ScrollView;

        private ScrollRect ScrollRect;

        // private readonly string TAG = "Chapter";

        private MainSystem mainSystem;
        private UISystem UISystem;
        private CameraController cameraController;

        // 更新触摸事件并调度触摸事件
        private void UpdateTouchEvents()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPoint = touch.rawPosition;
                    Ray ray = Camera.main.ScreenPointToRay(touchPoint);

                    CatchRay(ray);
                }
            }
        }
        // 解析触摸事件
        private void CatchRay(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                // if (hitObject.tag != TAG) return;       // 处理点击对象

                // 如果点击到
                Determine(hitObject);
            }  
        }

        // 单点判定
        private void Determine(GameObject hitObject)
        {
            // 选择章节入口
            if (hitObject.tag == "Chapter" && mainSystem.PlayerStatus == 0) 
            {
                ChapterTab tab = hitObject.GetComponent<ChapterTab>();
                Chapter chapter = mainSystem.chapters[tab.ChapterIndex];
                    
                // 终止相机移动，防干扰
                // StopCoroutine(GetComponent<CameraController>().Ie);

                // 这里做进入章节详情页
                mainSystem.ChapterSelected(chapter);
            }
            // 切换难度的入口（未完成）
            else if (hitObject.tag == "LevelSwitch" && mainSystem.PlayerStatus == 1)
            {
                hitObject.GetComponent<LevelSwitcher>().SwitchLevel();
            }
            // 切换关卡入口
            else if (hitObject.tag == "Level" && mainSystem.PlayerStatus == 1)
            {
                LevelTab tab = hitObject.GetComponent<LevelTab>();
                Level level = tab.level;
                GetComponent<LevelHandler>().SelectLevel(level);
            }
            // 进入关卡的入口
            else if (hitObject.tag == "Level_selected" && mainSystem.PlayerStatus == 1)
            {
                mainSystem.StartGame(hitObject.GetComponent<LevelTab>().level);
            }
        }

        // 手指滑动
        private UnityAction<Vector2> action;
        private void ScrollView_onValueChanged(Vector2 action)
        {
            if (mainSystem.PlayerStatus == 0)
            {
                cameraController.UpdateCameraPosition(action.y);
            }
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                UpdateTouchEvents();
            }
        }

        private void Start()
        {
            mainSystem = GetComponent<MainSystem>();
            UISystem = GetComponent<UISystem>();
            cameraController = GetComponent<CameraController>();

            ScrollRect = ScrollView.GetComponent<ScrollRect>();

            action += ScrollView_onValueChanged;
            ScrollRect.onValueChanged.AddListener(action);
        }
    }
}