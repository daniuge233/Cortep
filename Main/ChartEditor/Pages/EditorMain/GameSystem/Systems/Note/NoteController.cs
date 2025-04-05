using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    // 负责协助Note.cs处理MonoBehaviour任务
    // 这个脚本挂载在MainNotes上
    public class NoteController : MonoBehaviour
    {
        // note.cs
        public Note Note;

        public IEnumerator FadeIn(List<GameObject> objs)
        {
            while (objs[0].GetComponent<SpriteRenderer>().color.a < 1)
            {
                foreach (GameObject obj in objs)
                {
                    Color color = obj.GetComponent<SpriteRenderer>().color;
                    obj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, color.a + Time.deltaTime * 5f);
                }   

                yield return null; 
            }

            yield return null;
        }

        // 为Note.cs保留的启动和停止协程函数的方法
        public Coroutine StartCorout(IEnumerator Ie)
        {
            return StartCoroutine(Ie);
        }
        public void StopCorout(Coroutine Ie)
        {
            StopCoroutine(Ie);
        }
        // 保留的销毁方法
        public void destroy(UnityEngine.Object obj)
        {
            Destroy(obj);
        }
    }
}