using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ChartEditor.Project
{
    public class Project
    {
        public string Name;
        public string Author;
        public string Chart;
        public string Illustration;
        public string MusicFolder;
        public string IllustrationFolder;
        public string BPM;
        public string Difficulty;

        public List<Ring> Rings;
        public List<Event> Events;
    }
}