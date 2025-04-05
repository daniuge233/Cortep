using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomePage
{
    // 这是对主界面章节的定义
    public class Chapter
    {
        public MainSystem mainSystem;

        private int ChapterID;

        private string ChapterName;

        private GameObject father;
        private GameObject ring_i, ring_o;
        private GameObject nameTxt;

        private GameObject Title;
        private GameObject Author;
        private GameObject Level;

        // 该章节下包含的关卡
        // MainSystem无法直接控制关卡，需要通过Chapter间接控制
        private List<Level> levels = new();     

        private readonly Vector3[] ps = new Vector3[8] {
                new Vector3(0, 3.2f, 0), new Vector3(2.2f, 2.2f, 0),
                new Vector3(3.2f, 0, 0), new Vector3(2.2f, -2.2f, 0),
                new Vector3(0, -3.2f, 0), new Vector3(-2.2f, -2.2f, 0),
                new Vector3(-3.2f, 0, 0), new Vector3(-2.2f, 2.2f, 0),
        };

        public Chapter(MainSystem mainSystem, int i, string ChapterName, List<Level> levels)
        {
            // 创建Objects
            ring_i = new("ring", typeof(SpriteRenderer));
            ring_o = new("ring", typeof(SpriteRenderer));
            father = new("ring_f", typeof(SpriteRenderer));

            Title = new("Title", typeof(TextMeshPro), typeof(LevelInfoIniter));
            Author = new("Author", typeof(TextMeshPro), typeof(LevelInfoIniter));
            Level = new ("Level", typeof(TextMeshPro), typeof(LevelInfoIniter));

            // 设置Tag
            ring_o.tag = "Chapter";
            
            // 设置文字
            nameTxt = new("ring_name");

            // 初始化材质
            ring_i.GetComponent<SpriteRenderer>().sprite = ring_o.GetComponent<SpriteRenderer>().sprite = mainSystem.ring;
            
            // 初始化文字
            TextMeshPro tmp = nameTxt.AddComponent<TextMeshPro>();
            InitChapterName(tmp, mainSystem, ChapterName);

            // 初始化外圈
            ring_o.transform.localScale = new Vector3(1.75f, 1.75f, 1);     // 根据关卡部分MainSystem.DrawRing定义，外圆半径是内圆的1.75倍
            ring_o.AddComponent<ChapterTab>().ChapterIndex = i;

            // 设置父元素
            nameTxt.transform.SetParent(mainSystem.canvas.transform);
            InitParents(new List<GameObject> {ring_i, ring_o, Title, Author, Level}, father);

            // 添加Collider
            ring_o.AddComponent<SphereCollider>();

            // 初始化Levels
            InitLevels(levels, father);

            // 初始化Class
            ChapterID = i;
            this.mainSystem = mainSystem;
            this.ChapterName = ChapterName;
            this.levels = levels;
        }

        private void InitChapterName(TextMeshPro tmp, MainSystem mainSystem, string ChapterName)
        {
            tmp.text = ChapterName;
            tmp.font = mainSystem.font;
            tmp.font.material.mainTexture.filterMode = FilterMode.Point;
            tmp.color = Color.black;
            tmp.alignment = TextAlignmentOptions.Right;
            tmp.fontSize = 10;
        }

        // 初始化章节中间信息显示部分的父元素设置
        private void InitParents(List<GameObject> objs, GameObject parent)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].transform.SetParent(parent.transform);
            }
        }

        // 初始化该章节的所有levels
        public void InitLevels(List<Level> levels, GameObject father)
        {
            int bound = levels.Count;
            for (int i = 0; i < bound; i++)
            {
                GameObject parent = levels[i].parent;
                parent.transform.SetParent(father.transform);

                parent.transform.localPosition = ps[i];
            }
        }

        public GameObject GetFather() { return father; }
        public GameObject GetOutRing() { return ring_o; }
        public GameObject GetNameTxt() { return nameTxt; }

        public LevelSwitcher GetLevelSwitcher() { return Level.GetComponent<LevelSwitcher>(); }

        public string GetChapterName() { return ChapterName; }
        public int GetChapterID() { return ChapterID; }
        public List<Level> GetLevelsList() { return levels; }

        // 选中曲目
        public void SelectLevel(int LevelID)
        {
            Level level = levels[LevelID];

            Title.GetComponent<TextMeshPro>().text = level.Name;
            Author.GetComponent<TextMeshPro>().text = level.Author;

            // StartCoroutine函数必须在MonoBehaviour脚本中运行，所以这里用LevelTab启动协程
            // 以后记住！StopCoroutine传入的参数是Coroutine类型！
            // 绝对不能用IEnumerator停止协程！！！！！！
            if (level.AnimIe != null) level.GetTab().StopCoroutine(level.AnimIe);
            level.AnimIe = level.GetTab().StartCoroutine(LevelAnimation(level, new Vector3(0, 0, -0.3f)));   

            // 需要将List<int>类型的难度数据转为string
            List<int> dif = level.Difficulties;
            string Difficulties = "";
            for (int i = 0; i < dif.Count; i++) 
            {
                Difficulties += dif[i].ToString();
                if (i < dif.Count - 1) Difficulties += "  ";    
            }
            Level.GetComponent<TextMeshPro>().text = Difficulties;
        }

        public IEnumerator LevelAnimation(Level level, Vector3 target)
        {
            GameObject Ill = level.Ill;
            GameObject Mask = level.Mask;

            // Vector3 target = new Vector3(0, 0, -0.75f);
            Vector3 vec = Vector3.zero;

            while (true)
            {
                Ill.transform.localPosition = Mask.transform.localPosition = 
                Vector3.SmoothDamp(Ill.transform.localPosition, target, ref vec, 0.05f);

                yield return null;
            }
        }

        public void ClearLevelInfo()
        {
            Title.GetComponent<TextMeshPro>().text = Level.GetComponent<TextMeshPro>().text = Author.GetComponent<TextMeshPro>().text = "";
        }

        // 用于进入关卡的时候
        // 使关卡信息消失
        public IEnumerator FadeOutLevelInfo()
        {
            TextMeshPro title = Title.GetComponent<TextMeshPro>(),
                        author = Author.GetComponent<TextMeshPro>(),
                        level = Level.GetComponent<TextMeshPro>();
            float tar = 0, curVect = 0;

            while (title.color.a > 0)
            {
                title.color = author.color = level.color = 
                new Color(255, 255, 255, Mathf.SmoothDamp(title.color.a, tar, ref curVect, 0.05f));

                yield return null;
            }

            yield return null;
        }
    }
}