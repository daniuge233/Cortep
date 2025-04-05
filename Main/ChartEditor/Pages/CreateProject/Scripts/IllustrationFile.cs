using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;
using SFB;

namespace ChartEditor.CreatProject
{
    public class IllustrationFile : MonoBehaviour
    {
        public Button Trigger;

        public string path;

        public void Click()
        {
            var extensions = new [] {
                new ExtensionFilter("Image Files", "png", "jpg")
            };

            path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true)[0];

            if (path == "") return;

            ((TMP_Text)gameObject.GetComponentsInChildren(typeof(TMP_Text))[0]).text = "Selected";
        }
    }
}