using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Project
{
    public class Note
    {
        public enum NoteType
        {
            Duang,
            Full,
            Licky,
            Mining
        }

        public int ID;
        public NoteType Type;
        public int[] Args;
    }
}