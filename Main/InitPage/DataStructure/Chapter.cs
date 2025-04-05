using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeserializaedData
{
    [Serializable]
    public class Chapter
    {
        public int ID;
        public string Name;

        public List<Level> Levels;
    }
}
