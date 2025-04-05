using System;
using System.Collections;
using UnityEngine;

namespace HomePage
{
    public class CameraController : MonoBehaviour
    {
        private MainSystem mainSystem;

        // public Coroutine Ie;     // 存储协程以方便启动和结束

        public int PSchedule = 0;      // 存储相机停留的上一个位置，用于判断相机是否抵达。
        public int Schedule = 0;     // 移动进度(0~100)

        public bool isActive = true;
        
        public void UpdateCameraPosition(float current)
        {
            if (isActive)
            {
                float z = 0;
                float x = 0;

                // 当前移动进度，可用作x
                // 乘6是因为x分度值为6
                float CurChapter = (mainSystem.ChapterNT - 1) * current * 6;
                x = -0.0125f * CurChapter * CurChapter;
                // 当前进度，可用作z
                // -7.5f是因为相机和章节之间的距离为7.5，具体参考MainSystem
                z = (mainSystem.ChapterNT - 1) * current * 20 - 7.5f;        

                gameObject.transform.position = new Vector3(-1, x, z);
            }
        }

        private void Start()
        {
            mainSystem = GetComponent<MainSystem>();
            //cameraShake = GetComponent<CameraShake>();
        }
    }
}