using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomePage
{
    public class Level
    {
        // 这两个变量构成了「全局索引」
        // 所有每个章节的Level信息是由Chapter控制的
        // 所以可以用Chapter.levels获取到Level
        public int ID;      // Level在当前Chapter中的ID
        public int FatherChapterID;     // 当前Level所在Chapter的ID

        public Sprite Illustration;         // 曲绘
        public List<int> Difficulties = new();      // 难度

        public string Name, Author, Illustrator, Chart;

        private MainSystem mainSystem;      // ms

        private Sprite Round;

        public GameObject parent;
        public GameObject Ill;
        public GameObject Mask;

        private LevelTab tab;       // 该level的tab

        public Coroutine AnimIe;

        public int CurrentSelectedDifficulty = 0;       // 当前选择的难度

        public Level(
            int ID, int FatherChapterID, Sprite Illustration, string Name, string Author, string Illustrator, string Chart,
            List<int> Difficulties, MainSystem mainSystem
        ) 
        {
            this.ID = ID;
            this.FatherChapterID = FatherChapterID;
            this.Illustration = Illustration;
            this.Name = Name;
            this.Author = Author;
            this.Illustrator = Illustrator;
            this.Chart = Chart;
            this.Difficulties = Difficulties;
            this.mainSystem = mainSystem;

            Round = mainSystem.Round;

            InitObjects();
        }

        // 初始化对象
        private void InitObjects() 
        {
            parent = new GameObject("Level_Parent", typeof(SphereCollider), typeof(SpriteRenderer), typeof(LevelTab));
            Ill = new GameObject("Level_Illustration", typeof(SpriteRenderer));
            Mask = new GameObject("Level_Illustration_Mask", typeof(SpriteMask));
            Ill.transform.SetParent(parent.transform);
            Mask.transform.SetParent(parent.transform);

            parent.tag = "Level";
            parent.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Mask.transform.localScale = new Vector3(0.85f, 0.85f, 1);
            Ill.transform.position = Mask.transform.localPosition = new Vector3(0, 0, -0.0001f);

            int MaskID = FatherChapterID * 10 + ID;

            SpriteRenderer sr = Ill.GetComponent<SpriteRenderer>();
            sr.sprite = Illustration;
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            sr.sortingOrder = MaskID;
            SpriteMask mask = Mask.GetComponent<SpriteMask>();
            mask.sprite = Round;
            mask.isCustomRangeActive = true;
            mask.frontSortingOrder = MaskID;
            mask.backSortingOrder = MaskID - 1;

            parent.GetComponent<SpriteRenderer>().sprite = mainSystem.Round;
            // parent.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            parent.GetComponent<SphereCollider>().radius = 0.75f;
            tab = parent.GetComponent<LevelTab>();
            tab.LevelID = ID;
            tab.ParentChapterID = FatherChapterID;
            tab.mainSystem = mainSystem;
            tab.level = this;
        }

        public LevelTab GetTab() { return tab; }
    }
}