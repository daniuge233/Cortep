using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // Mining(长按音符)的定义
    public class Mining : Note
    {
        private int HoldTime;
        private float current;

        // Mining的子对象
        private GameObject inside;

        private bool Begin = false;

        // HoldTime: Mining按住时间,单位毫秒(ms).
        public Mining(int RingID, int index, int ActiveBeat, int HoldTime) : base(RingID, index, NoteType.Mining, ActiveBeat)
        {
            this.HoldTime = HoldTime;
            InitInside();
        }

        private void InitInside()
        {
            inside = new("Mining_Inside", typeof(SpriteRenderer));
            SetParent(new() {inside});
            inside.GetComponent<SpriteRenderer>().sprite = DB.Mining_Inside;
            inside.transform.localScale = new Vector3(0, 0, 0);
        }

        // 特效播放调度函数
        private float T = 0.75f;
        private void UpdateEffect()
        {
            T += Time.deltaTime;

            if (T >= 0.75f)
            {
                T = 0;
                NoteHandler.PlayEffect(this, result);
            }
        }

        public override void Click() 
        {
            Begin = true;
            result = Arbiter.Arbit(this);
        }

        public override void Hold() 
        {
            isHold = true;
            current += Timer.deltaMilSec;

            float process = current / HoldTime;
            inside.transform.localScale = new Vector3(process, process, 0);

            if (current >= HoldTime)
            {
                FinishNote();
            }

            UpdateEffect();
        }

        // 检测是否提前松开
        // Miss
        public override void Release() 
        {
            if (Begin && !isDestroyed)
            {
                result = ArbitingData.ArbitingStatus.BadMiss;
                FinishNote();
            }
        }
    }
}