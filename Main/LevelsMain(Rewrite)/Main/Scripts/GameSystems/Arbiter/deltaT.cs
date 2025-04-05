using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // deltaT定义
    // 包含判定线两端到一Note的时间差值
    // 使用时应判断大小以确定使用哪个
    public class deltaT
    {
        public readonly float PositiveDeltaT;
        public readonly float NegativeDeltaT;

        public deltaT(float PositiveDeltaT, float NegativeDeltaT)
        {
            this.PositiveDeltaT = PositiveDeltaT;
            this.NegativeDeltaT = NegativeDeltaT;
        }

        // 获取正确的deltaT
        // 返回Positive和Negative中的最小值
        public float GetFixedDeltaT() { return Mathf.Min(PositiveDeltaT,  NegativeDeltaT); }
    }
}
