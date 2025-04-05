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

namespace ChartEditor.CreatProject
{
    public class Commit : MonoBehaviour
    {
        public MusicFile Mfile;
        public IllustrationFile IFile;

        [Space]
        public TMP_InputField Name;
        public TMP_InputField Author;
        public TMP_InputField Chart;
        public TMP_InputField Illustration;
        public TMP_InputField BPM;
        public TMP_InputField Difficulty;

        // private AudioClip MusicClip;

        // 真正的创建项目的函数
        // 看上面的using就知道这beta玩意多麻烦了（
        public void Select()
        {
            // 收集到的所有输入数据
            string
                name = Name.text,
                author = Author.text,
                chart = Chart.text,
                illustration = Illustration.text,
                bpm = BPM.text,
                difficulty = Difficulty.text,
                mpath = Mfile.path,
                ipath = IFile.path;

            // 确定无遗漏
            if (name == "" || author == "" || chart == "" || illustration == "" || bpm == "" || difficulty == "" || mpath == "" || ipath == "")
            {
                return;
            }

            // 创建Project对象
            Project.Project proj = new ()
            {
                Name = name,
                Author = author,
                Chart = chart,
                Illustration = illustration,
                MusicFolder = mpath,
                IllustrationFolder = ipath,
                BPM = bpm,
                Difficulty = difficulty
            };
            // StartCoroutine(FileToClip(mpath));
            // proj.Music = MusicClip;
            // proj.IllustrationSprite = FileToSprite(ipath);

            // 写入项目json文件
            // List对象无法被Unity序列化，使用JsonUtility会出现无法转换的问题。所以使用Newtonsoft.Json
            // string jsonString = JsonUtility.ToJson(proj);
            string jsonString = JsonConvert.SerializeObject(proj);

            string path = Application.streamingAssetsPath + "/ChartEditor/Projects/" + name + ".json";

            // 检测项目重复
            if (File.Exists(path))
            {
                ((TMP_Text)gameObject.GetComponentsInChildren(typeof(TMP_Text))[0]).text = "Project Already Exists";
            } 
            else
            {
                WriteFile(path, jsonString);

                // 将项目标记，方便管理
                string cachePath = Application.streamingAssetsPath + "/ChartEditor/Cache/Data.json";
                // 然而，JsonUtility.FromJson似乎不会出现上面说的问题
                // Cache.Cache cache = JsonUtility.FromJson<Cache.Cache>(File.ReadAllText(cachePath));
                // 不，我错了，它会（（（
                Cache.Cache cache = JsonConvert.DeserializeObject<Cache.Cache>(File.ReadAllText(cachePath));
                cache.Projects.Add(new Cache.Project()
                {
                    Name = name,
                    Path = "/ChartEditor/Projects/" + name + ".json"
                });
                string cacheString = JsonConvert.SerializeObject(cache);
                WriteFile(cachePath, cacheString);
            }

            Database.Database.EditingProject = proj;
            SceneManager.LoadScene("Main/ChartEditor/Pages/EditorMain/General/General");
        }

        private void WriteFile(string path, string content)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            File.WriteAllText(path, content);
        }



        // public IEnumerator FileToClip(string path)
        // {
        //     if (path == "") yield break;
            
        //     string url = "file://" + path;
        //     using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        //     {
        //         yield return www.SendWebRequest();

        //         if (www.result == UnityWebRequest.Result.Success)
        //         {
        //             MusicClip = DownloadHandlerAudioClip.GetContent(www);
        //         }
        //     }
        // }

        // public Sprite FileToSprite(string filePath)
        // {
        //     if (File.Exists(filePath))
        //     {
        //         byte[] fileData = File.ReadAllBytes(filePath);
                
        //         Texture2D texture = new Texture2D(2, 2);
        //         if (texture.LoadImage(fileData))
        //         {
        //             return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //         }
        //     }
            
        //     return null;
        // }
    }
}