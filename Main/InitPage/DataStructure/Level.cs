using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeserializaedData
{
    [Serializable]
    public class Level
    {
        public int ID;
        public string Name;
        public string Author;
        public string Illustration;
        public string Chart;
        
        public List<int> Difficulties;
    }
}
