using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 由于允许同时存在多个Rings，所以每个Ring必须单独配备一个判定器（每个Ring的判定线状态可能不一样）。
    // 和老版本关卡系统不同，这里的Arbiter不负责触摸，只负责判定。
    // (甚至判定都是由ArbitingData完成的)
    public class Arbiter : MonoBehaviour
    {
        private GameObject AssessLine;

        private void OnEnable()
        {
            AssessLine = gameObject.transform.Find("AssessLine").gameObject;
        }
        
        // 判定器主函数
        // 每个Note灵活调用该函数以达成判定目的。
        // 因为不同Note有不同的判定逻辑，所以统一判定器并不是好办法。
        public static ArbitingData.ArbitingStatus Arbit(Note note)
        {
            return new ArbitingData(note).Arbit();
        }

        // Inspector里显示的EulerAngles和transform.eulerAngles不一样！！！！！！
        // 傻逼啊啊啊啊啊啊啊啊啊啊啊啊！
        // 用于eulerAngles转Inspector
        // 摘自CSDN（
        public static Vector3 EulerAngles2InspectorRotation(Vector3 up, Vector3 eulerAngle)
        {
            Vector3 resVector = eulerAngle;
 
            if (Vector3.Dot(up, Vector3.up) >= 0f)
            {
                if (eulerAngle.x >= 0f && eulerAngle.x <= 90f)
                    resVector.x = eulerAngle.x;
 
                if (eulerAngle.x >= 270f && eulerAngle.x <= 360f)
                    resVector.x = eulerAngle.x - 360f;
            }
 
            if (Vector3.Dot(up, Vector3.up) < 0f)
            {
                if (eulerAngle.x >= 0f && eulerAngle.x <= 90f)
                    resVector.x = 180 - eulerAngle.x;
 
                if (eulerAngle.x >= 270f && eulerAngle.x <= 360f)
                    resVector.x = 180 - eulerAngle.x;
            }
 
            if (eulerAngle.y > 180)
                resVector.y = eulerAngle.y - 360f;
 
            if (eulerAngle.z > 180)
                resVector.z = eulerAngle.z - 360f;

            return resVector;
        }

        // 计算时间差值
        // 返回deltaT，详见deltaT.cs
        public static deltaT DeltaR2DeltaT(Note note, AssessLine asl)
        {
            float NoteRotation = Mathf.Abs(EulerAngles2InspectorRotation(note.MainNote.transform.up, note.MainNote.transform.eulerAngles).z);
            float AslRotation = Mathf.Abs(EulerAngles2InspectorRotation(asl.gameObject.transform.up, asl.gameObject.transform.eulerAngles).z);

            return new deltaT(Mathf.Abs(NoteRotation - AslRotation) / asl.GetSpeed() * 1000, Mathf.Abs(AslRotation - 180 - NoteRotation) / asl.GetSpeed() * 1000);
        }
    }
}