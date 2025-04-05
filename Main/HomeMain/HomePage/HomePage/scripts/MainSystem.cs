using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using DeserializaedData;

namespace HomePage
{
    // 主界面主系统
    // 负责章节管理、基础UI、相机运动等功能
    // 这次不是屎山代码力（喜）
    public class MainSystem : MonoBehaviour
    {
        public int PlayerStatus = 0;        // 默认为主菜单，0:主菜单，1:章节页

        public int ChapterNT;       // 章节数量
        public int ChaptersListNT;        // 章节表数量(每个表有两个章节)

        public GameObject canvas;       // 画布
        public GameObject UI_Canvas;        // UI画布
        // UI画布和普通画布不同，它是屏幕空间覆盖模式，缩放模式应为屏幕大小缩放。
        public GameObject BG;       // 背景图片

        public Sprite ring;         // ring
        public Sprite Round;        // 圆形
        public TMP_FontAsset font;           // 字体
        public List<Chapter> chapters = new();        // 章节列表
        public List<Vector3> chapterPs = new();         // 章节位置（用于相机移动）

        public List<Sprite> Sprites = new();       // 所有材质（0:返回按钮）

        // 每个章节列表的坐标增量
        private const float XRIGHT = 5, XLEFT = -5, XMID = 0;

        // 判断两点坐标是否相差在offset位小数内
        // 多用于判断SmoothDamp之类的非线性插值函数是否到达
        // 若前offset位小数相等即返回true
        // 否则返回false
        // 记得using System;！
        public bool isAchieve(Vector3 origin, Vector3 target, int offset = 2) 
        {

            if (Math.Round(target.x, offset) == Math.Round(origin.x, offset) && 
                Math.Round(target.y, offset) == Math.Round(origin.y, offset) && 
                Math.Round(target.z, offset) == Math.Round(origin.z, offset))
            {

                return true;
            }

            return false;
        }

        // 相机移入的动画
        private IEnumerator CameraMoveIn()
        {
            transform.position = new Vector3(0, 50, -10);
            while (transform.position.y >= 0.15f)        // 这里取近似值到0.1f，足够了
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(-1, 0, -10), 3f * Time.deltaTime);
                yield return null;
            }

            GetComponent<UserController>().ScrollView.SetActive(true);
            // GetComponent<CameraShake>().isActive = true;
            yield return null;
        }

        // 章节排列初始化
        private void InitChapters()
        {
            float z = 7.5f;
            float x = 0;

            GameObject[] Database = GameObject.FindGameObjectsWithTag("Database");
            Init db = Database[0].GetComponent<Init>();
            DeserializaedData.Data InitData = db.InitData;
            ChapterNT = InitData.Chapters.Count;

            // y = -0.0125x^2是章节y坐标的函数
            // x分度值为6
            // 实际使用的x = 7.5 + n * 20
            // n为当前章节id（从0开始）
            for (int i = 0; i < ChapterNT; i++)
            {
                float function = -0.0125f * x * x;

                List<DeserializaedData.Level> level = InitData.Chapters[i].Levels;
                List<Level> levels = new();

                foreach (DeserializaedData.Level L in level)
                {
                    Level l = new Level(L.ID, InitData.Chapters[i].ID, default(Sprite), L.Name, L.Author, L.Illustration, L.Chart, L.Difficulties, this);
                    levels.Add(l);
                }

                chapters.Add(new Chapter(this, InitData.Chapters[i].ID, InitData.Chapters[i].Name, levels));

                // 调整圆环和标题的位置
                chapters[i].GetFather().transform.position = new Vector3(XRIGHT, function, z);
                chapters[i].GetNameTxt().transform.position = new Vector3(XLEFT - 10, function, z);

                chapterPs.Add(new Vector3(-1, function, z));

                x += 6;
                z += 20;
            }
        }

        // 用户选中章节
        private Coroutine SelectedTransIe;
        
        // private GameObject DEV_BTN;

        public void ChapterSelected(Chapter chapter)
        {
            PlayerStatus = 1;
            StopAllCoroutines();
            GetComponent<CameraController>().isActive = false;     // 气死我了
            GetComponent<UserController>().ScrollView.SetActive(false);

            Destroy(chapter.GetOutRing().GetComponent<SphereCollider>());       // 防干扰

            GameObject chapterFather = chapter.GetFather();

            // 返回按钮
            GameObject Return = GetComponent<UISystem>().CreateButton(Sprites[0], 0, chapter.GetChapterID());
            Vector3 Target = new Vector3(0, chapterFather.transform.position.y + 20, chapterFather.transform.position.z);
            // Return.transform.position = new Vector3(-340, 140, 0);

            Return.GetComponent<RectTransform>().anchoredPosition = new Vector3(-900, 350, 0);
            Return.transform.localScale = new Vector3(0.5f, 0.5f, 1);

            // DEV_BTN = Return;

            // 播放过度动画（协程函数）
            SelectedTransIe = StartCoroutine(SelectedTrans(chapterFather));
        }

        // 选中后章节和相机运动的协程
        private IEnumerator SelectedTrans(GameObject chapterFather) 
        {
            Vector3 curVelocity_chapter = Vector3.zero, curVelocity_camera = Vector3.zero;
            Vector3 Target = new Vector3(0, chapterFather.transform.position.y + 20, chapterFather.transform.position.z);

            GameObject camera = gameObject;

            Vector3 CameraTarget = new Vector3(camera.transform.position.x, Target.y, Target.z - 10);

            while (!(
                isAchieve(chapterFather.transform.position, Target, 1) && 
                isAchieve(camera.transform.position, CameraTarget, 1)
            )) 
            {
                chapterFather.transform.position = Vector3.SmoothDamp(chapterFather.transform.position, Target, ref curVelocity_chapter, 0.15f);
                camera.transform.position = Vector3.SmoothDamp(camera.transform.position, CameraTarget, ref curVelocity_camera, 0.2f);

                yield return null;
            }

            StopCoroutine(SelectedTransIe);
            yield return null;
        }
        
        // 调度按钮点击事件
        public void ButtonEvent(GameObject evt)
        {
            ButtonTab tab = evt.GetComponent<ButtonTab>();
            if (tab.type == 0) ReturnEvent(evt);
        }

        // 调度返回章节列表请求
        private Coroutine UpdateReturnEventIe;
        public void ReturnEvent(GameObject evt) 
        {
            if (PlayerStatus == 1)
            {
                PlayerStatus = 0;

                // cc.PSchedule = cc.Schedule;
                // cc.Update_();
                StopAllCoroutines();
                int index = evt.GetComponent<ButtonTab>().index;
                UpdateReturnEventIe = StartCoroutine(UpdateReturnEvent(index, evt));
                StartCoroutine(GetComponent<UISystem>().FadeOut(evt.GetComponent<Image>(), 0.1f, true));
            }
        }

        // 处理返回章节列表请求
        private IEnumerator UpdateReturnEvent(int index, GameObject evt) 
        {
            Vector3 curVelocity_chapter = Vector3.zero, curVelocity_camera = Vector3.zero;

            Chapter chapter = chapters[index];      // 根据Tab中的index获取到chapter
            GameObject chapter_father = chapter.GetFather();        // 根据chapter获取其父元素  
            Vector3 chapter_target = chapterPs[index];      // 根据index获取父元素目标位置
            chapter_target.x += 5;

            GameObject camera = gameObject;
            Vector3 camera_target = new Vector3(camera.transform.position.x, chapter_target.y, chapter_target.z - 17.5f);
            
            LevelHandler levelHandler = GetComponent<LevelHandler>();
            if (levelHandler.CanDeSelect()) levelHandler.DeSelect();

            chapter.GetOutRing().AddComponent<SphereCollider>();

            // Destroy(evt);       // 删除返回按钮

            while (
                !(isAchieve(chapter_father.transform.position, chapter_target, 1) && isAchieve(camera.transform.position, camera_target, 1))
            )
            {
                chapter_father.transform.position = Vector3.SmoothDamp(chapter_father.transform.position, chapter_target, ref curVelocity_chapter, 0.15f);
                camera.transform.position = Vector3.SmoothDamp(camera.transform.position, camera_target, ref curVelocity_camera, 0.2f);

                yield return null;
            }

            // 重启CameraController
            GetComponent<CameraController>().isActive = true;
            GetComponent<UserController>().ScrollView.SetActive(true);

            yield return null;
        }
        
        // 启动关卡的调度函数
        public void StartGame(Level level)
        {
            GetComponent<CameraController>().enabled =
            GetComponent<UserController>().enabled =
            GetComponent<ParticleController>().enabled = 
            GetComponent<LevelHandler>().enabled = 
            GetComponent<UISystem>().enabled = false;

            StopAllCoroutines();
            // StopCoroutine(GetComponent<UISystem>().SliderUpdater);

            Chapter chapter = chapters[level.FatherChapterID];
            StartCoroutine(chapter.FadeOutLevelInfo());
            StartCoroutine(StartingCameraMovement());
            StartCoroutine(GetComponent<UISystem>().FadeOut(GameObject.Find("button").GetComponent<Image>(), 0.1f, true));
            
            GameObject PHandler = GetComponent<ParticleController>().ParticleHandler;
            PHandler.transform.position = new Vector3(PHandler.transform.position.x, PHandler.transform.position.y, PHandler.transform.position.z - 9999);
        }

        // 开始游戏后，相机前移
        // 这块动画太慢了！！！！！！
        private IEnumerator StartingCameraMovement()
        {
            GameObject camera = gameObject;
            Vector3 target = new(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + 150);
            Vector3 curVect = Vector3.zero;

            while (!isAchieve(camera.transform.position, target, 1))
            {
                camera.transform.position = Vector3.SmoothDamp(camera.transform.position, target, ref curVect, 0.35f);

                yield return null;
            }

            yield return null;
        }

        // 主系统的初始化操作必须在最开始
        // 这就是主系统的地位（bs
        private void Awake()
        {
            ChaptersListNT = ChapterNT - 1;
            StartCoroutine(CameraMoveIn());
            InitChapters();

            // 无限制帧率
            Application.targetFrameRate = -1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z)) ChapterSelected(chapters[0]);
        }
    }
}
