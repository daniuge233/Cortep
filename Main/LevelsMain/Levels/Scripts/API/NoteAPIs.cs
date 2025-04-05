using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelsMain
{
    public class NoteAPIs
    {
        private MainSystem mainSystem;

        public NoteAPIs(MainSystem mainSystem)
        {
            this.mainSystem = mainSystem;
        }

        // 添加音符API
        public void AddNote(int type, int ActiveTime, int PositionID, int waitTime, int length, int holdTime, int ActiveKeep)
        {
            mainSystem.AddNote(type, ActiveTime, PositionID, waitTime, length, holdTime, ActiveKeep);
        }
    }
}
