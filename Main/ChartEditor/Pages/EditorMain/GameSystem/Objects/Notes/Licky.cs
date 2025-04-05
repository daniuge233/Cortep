using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    // Licky(轻划音符)的定义
    public class Licky : Note
    {
        public Licky(int RingID, int index, int ActiveBeat) : base(RingID, index, NoteType.Licky, ActiveBeat) {}

        public override void Click() {}
        public override void Hold() {}
        public override void Release() {}
    }
}