using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // Duang(单点音符)的定义
    // 这是逻辑最简单的音符((
    public class Duang : Note
    {
        public Duang(int RingID, int index, int ActiveBeat) : base(RingID, index, NoteType.Duang, ActiveBeat) {}

        public override void Click()
        {
            FinishNote();
        }

        public override void Hold() {}
        public override void Release() {}
    }
}