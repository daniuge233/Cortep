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
    public class MusicFile : MonoBehaviour
    {
        public Button Trigger;

        public string path;

        public void Click()
        {
            var extensions = new [] {
                new ExtensionFilter("Sound Files", "mp3")
            };

            path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true)[0];

            // StartCoroutine(FileToClip(paths[0]));   
            
            if (path == "") return;

            ((TMP_Text)gameObject.GetComponentsInChildren(typeof(TMP_Text))[0]).text = "Selected";
        }

        // private IEnumerator FileToClip(string path)
        // {
        //     if (path == "") yield break;
            
        //     string url = "file://" + path;
        //     using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        //     {
        //         yield return www.SendWebRequest();

        //         if (www.result == UnityWebRequest.Result.Success)
        //         {
        //             AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
        //         }
        //     }
        // }
    }
}