using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 判定线的定义
    public class AssessLine : MonoBehaviour
    {
        public float CurrentRotation;

        private bool isActive = false;
        private float RotatePerSec = 0;
        
        // 启动判定线旋转的函数
        public void ActiveAssessLine()
        {
            isActive = true;
        }

        // 重置判定线
        public void ResetAssessLine()
        {
            isActive = false;
            gameObject.transform.eulerAngles = Vector3.zero;
        }

        // 切换判定线运动速度
        // 单位：度/拍
        public void UpdateAssessLine(float speed)
        {
            RotatePerSec = speed;
        }

        private void Update()
        {
            if (!isActive) return;

            float z = gameObject.transform.eulerAngles.z;
            CurrentRotation = z + RotatePerSec * Timer.deltaBPM;
            gameObject.transform.eulerAngles = new Vector3(0, 0, CurrentRotation);
        }

        public float GetSpeed() { return RotatePerSec; }
    }
}