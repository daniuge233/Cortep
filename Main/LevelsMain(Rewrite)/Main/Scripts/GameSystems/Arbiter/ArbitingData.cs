using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 方便API，这里单独做一个判定数据的class
    // 对于任意Note,这个脚本用于计算并判断延迟
    // 其实Arbiter.cs只是这个脚本的接口罢了
    // Arbiter只是它的替身!!!
    public class ArbitingData
    {
        private float Rotate;
        private float Speed;
        private Note note;


        // private float Difference_normal;
        // private float Difference_reverse;
        // float Delay_normal;
        // float Delay_reverse;

        // 时间差值(Ms)
        private float deltaT;
        private float fixedDeltaT;  // 在减去180后的deltaT计算结果

        // 所有判定结果
        public enum ArbitingStatus
        {
            Perfect,
            Good,
            BadMiss
        };
        
        public ArbitingData(Note note, GameObject gameObject = null)
        {
            this.note = note;
            // 自动根据note获取该note所在ring中的判定线
            AssessLine asl = note.GetRing().GetAssessLine().GetComponent<AssessLine>();

            GameObject noteObj = gameObject == null ? note.MainNote : gameObject;

            Rotate = Arbiter.EulerAngles2InspectorRotation(asl.gameObject.transform.up, asl.gameObject.transform.eulerAngles).z;
            float NoteRotate = Arbiter.EulerAngles2InspectorRotation(noteObj.transform.up, noteObj.transform.eulerAngles).z;

            Speed = asl.GetSpeed();

            // 角度差值
            float deltaR = Mathf.Abs(Rotate - NoteRotate);
            float fixedDeltaR = Mathf.Abs(Rotate - 180 - NoteRotate);
            // 时间差值(ms)
            deltaT = Timer.BeatToMs(Mathf.Abs(deltaR / Speed * 1000));
            fixedDeltaT = Timer.BeatToMs(Mathf.Abs(fixedDeltaR / Speed * 1000));
        }

        public float GetCurrentRotation() { return Rotate; }

        public ArbitingStatus Arbit()
        {
            // 三²目运算符（bs
            return (
                (deltaT <= DB.Perfect || fixedDeltaT <= DB.Perfect ?
                    ArbitingStatus.Perfect
                    : (deltaT <= DB.Good || fixedDeltaT <= DB.Good ? 
                        ArbitingStatus.Good : ArbitingStatus.BadMiss))
            );
        }
    }
}