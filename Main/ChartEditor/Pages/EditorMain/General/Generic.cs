using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ChartEditor.Main.GameSystem;

namespace ChartEditor.Main.Generic
{
    // 制谱器的集中管理中心
    // 这里负责谱面加载、Rings和事件的编辑等。
    // 游戏系统制谱器定制版的管理由其MainSystem负责。
    public class Generic : MonoBehaviour
    {
        public static AudioClip MusicClip;
        public static Sprite IllustrationSprite;
        public GameObject CoverPanel;

        public Main.GameSystem.MainSystem MainSystem;

        [Space]
        public GameObject 
            Content_Overriew,
            Content_RingEdit;

        [Space]
        public GameObject CurrentContent;

        private void Awake()
        {
            try
            {
                StartCoroutine(FileToClip(Database.Database.EditingProject.MusicFolder));
                IllustrationSprite = FileToSprite(Database.Database.EditingProject.IllustrationFolder);
            } catch (Exception e) {
                return;
            }
        }
        public IEnumerator FileToClip(string path)
        {
            if (path == "") yield break;
            
            string url = "file://" + path;
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    MusicClip = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }
        public Sprite FileToSprite(string filePath)
        {
            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData))
                {
                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
            
            return null;
        }

        public void CoverMainController()
        {
            CoverPanel.SetActive(true);
        }
        public void DiscoverMainController()
        {
            CoverPanel.SetActive(false);
        }

        public void LoadContent(string name)
        {
            CurrentContent.SetActive(false);
            switch (name)
            {
                case "Overview":
                    Content_Overriew.SetActive(true);
                    CurrentContent = Content_Overriew;
                    break;
                case "RingEdit":
                    Content_RingEdit.SetActive(true);
                    CurrentContent = Content_RingEdit;
                    break;
            }
        }

        // 从这里开始是Generic.cs对游戏系统的管理

        // 添加Ring
        public Main.GameSystem.Ring AddRing()
        {
            return new Main.GameSystem.Ring(MainSystem.RingCnt);
        }

        // 添加事件
        public void AddEvent()
        {

        }
    }
}