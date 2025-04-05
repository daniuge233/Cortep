using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;
using Newtonsoft.Json;

namespace ChartEditor.LoadProject
{
    public class ProjectsIniter : MonoBehaviour
    {
        public static Cache.Cache cache;
        public TMP_Dropdown Dropdown;

        private void Start()
        {
            cache = JsonConvert.DeserializeObject<Cache.Cache>(File.ReadAllText(Application.streamingAssetsPath + "/ChartEditor/Cache/Data.json"));

            for (int i = 0; i < cache.Projects.Count; i++)
            {
                Dropdown.options[i] = new TMP_Dropdown.OptionData(cache.Projects[i].Name);
            }
        }
    }
}