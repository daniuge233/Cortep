using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;
using Newtonsoft.Json;

namespace ChartEditor.LoadProject
{
    public class Commit : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;
        private Project.Project target;

        public void Select()
        {
            int value = Dropdown.value;
            string targetPath = ProjectsIniter.cache.Projects[value].Path;
            target = JsonConvert.DeserializeObject<Project.Project>(File.ReadAllText(Application.streamingAssetsPath + targetPath));

            Database.Database.EditingProject = target;
            SceneManager.LoadScene("Main/ChartEditor/Pages/EditorMain/General/General");
        }
    }
}