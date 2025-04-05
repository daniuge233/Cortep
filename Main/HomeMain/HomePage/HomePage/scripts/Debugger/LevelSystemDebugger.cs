using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomePage
{
    public class LevelSystemDebugger : MonoBehaviour
    {
        public Sprite illu;
        public string Name;
        public string Author;
        public string Illustrator;
        public string Chart;
        public List<int> Difficulties;

        public MainSystem mainSystem;
        private List<Chapter> chapters;

        public int curChapterID = 0;
        public int curID = 0;

        int i = 0;

        private void Start()
        {
            chapters = mainSystem.chapters;

            // for (int i = 0; i < 8; ++i)
            // {
            //     chapters[curChapterID].GetLevelsList().Add(new Level(curID, curChapterID, illu, Name, Author, Illustrator, Chart, Difficulties, mainSystem));
            //     chapters[curChapterID].InitLevels(chapters[curChapterID].GetLevelsList(), chapters[curChapterID].GetFather());
            //     curID++;
            //     if (curID >= 8) 
            //     {
            //         curChapterID++;
            //         curID = 0;
            //     }
            // }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                chapters[curChapterID].GetLevelsList().Add(new Level(curID, curChapterID, illu, Name, Author, Illustrator, Chart, Difficulties, mainSystem));
                chapters[curChapterID].InitLevels(chapters[curChapterID].GetLevelsList(), chapters[curChapterID].GetFather());
                curID++;
                if (curID >= 8) 
                {
                    curChapterID++;
                    curID = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                GetComponent<LevelHandler>().SelectLevel(chapters[0].GetLevelsList()[i++]);
                if (i == 8) i = 0;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                chapters[0].GetLevelSwitcher().SwitchLevel();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                StartCoroutine(chapters[0].FadeOutLevelInfo());
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                mainSystem.StartGame(chapters[0].GetLevelsList()[0]);
            }
        }
    }
}
