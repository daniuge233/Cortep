using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeserializaedData
{
    [Serializable]
    public class Data
    {
        public string Version;
        public string ReleaseDate;

        public List<Chapter> Chapters;
    }
}
