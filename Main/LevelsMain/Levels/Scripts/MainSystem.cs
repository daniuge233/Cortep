using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 什么屎山代码
//                 ▃▆█▇▄▖
//             ▟◤▖        ◥█▎
//        ◢◤    ▐              ▐▉
//     ▗◤         ▂   ▗▖      ▕█▎
//    ◤   ▗▅▖◥▄   ▀◣      █▊
// ▐   ▕▎◥▖◣◤            ◢██
// █◣   ◥▅█▀            ▐██◤
// ▐█▙▂              ◢██◤
// ◥██◣            ◢▄◤
//        ▀██▅▇▀

// 我希望让脚本的设置过程变得尽可能简洁, 所以这个脚本负责处理脚本挂载、卸载、调度以及处理Note的部分功能。
// 使用时仅需要将此脚本挂载至相机并设置好这个脚本, 其余的会自动处理。
// 这也是这个脚本会变成答辩山的原因之一。

// 最好不要一上来就加载所有note, 如果note很多的话可以边进行边加载。
// 我不希望在进入游戏前有几秒钟的加载时间

namespace LevelsMain
{
    public class MainSystem : MonoBehaviour
    {
        // 排序规则
        // 主要是为了给音符按启动时间排序
        private int Cmp(Note x, Note y)
        {
            return x.GetActiveTime().CompareTo(y.GetActiveTime());
        }

        public GameObject margin_in, margin_out;

        public GameObject mainParent;

        // 资源
        public AudioClip Music;     // 背景音乐
        public Sprite Square;     // 正方形
        public Sprite Ring;         // 圆环, 懒得算法生成了, 一张图片糊上去解决一切问题
        public Sprite[] NoteSprites = new Sprite[4];        // (0: duang   1: full(不要设置这个(设置了也没事反正用不上))   2: licky  3:mining)
        public Sprite mining_in; //mining的内圆
        public Sprite FullStart;    // full开始
        public Sprite FullEnd;      // full结束
        public Sprite FullArc;      // full音符的弧

        // 依赖库
        private AssessLine asl;     // 判定线系统
        private BPMTimer timer; // bpm计时器
        private MillisecondTimer msTimer;       // 毫秒计时器
        public static UISystem uis;       // UI系统
        private AudioSystem AudioS;      // 音频系统
        private Arbiter arbiter;        // 判定器

        // 字体
        public Font font;

        // 定位与被定位
        public List<Note> notes = new();                                             // 音符
        private readonly List<NoteVacancy> noteVacancys = new();       // 音符空位

        private bool isActive;

        // private ICN icn;

        // debug
        public GameObject dbg_txt;

        private void InitPositions()
        {
            // 所有note空位的坐标
            // 笨办法, 以后可以改成计算的
            Vector3[] ps = new Vector3[16] {
                new Vector3(0, 3.2f, 0), new Vector3(2.2f, 2.2f, 0),
                new Vector3(3.2f, 0, 0), new Vector3(2.2f, -2.2f, 0),
                new Vector3(0, -3.2f, 0), new Vector3(-2.2f, -2.2f, 0),
                new Vector3(-3.2f, 0, 0), new Vector3(-2.2f, 2.2f, 0),

                new Vector3(0, 1.5f, 0), new Vector3(1.1f, 1.1f, 0),
                new Vector3(1.5f, 0, 0), new Vector3(1.1f, -1.1f, 0),
                new Vector3(0, -1.5f, 0), new Vector3(-1.1f, -1.1f, 0),
                new Vector3(-1.5f, 0, 0), new Vector3(-1.1f, 1.1f, 0),
            };
            // 循环添加至note空位数据库
            for (int i = 1; i <= 16; i++)
            {
                noteVacancys.Add(new NoteVacancy(ps[i - 1], i, this));
            }
        }

        // 初始化音符(材质与位置)
        private void InitNote(GameObject obj, int noteType, int position)
        {
            // n.GetObj().transform.LookAt(zero.transform);
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.color = new Color(255, 255, 255, 0);
            switch (noteType)
            {
                case 0:
                    sr.sprite = NoteSprites[0];
                    break;
                // case 1:
                //     sr.sprite = NoteSprites[1];
                //     break;
                case 2:
                    sr.sprite = NoteSprites[2];
                    break;
                case 3:
                    sr.sprite = NoteSprites[3];
                    break;
            }
            if (position - 1 < 0)
            {
                position = 10 + position;
            }
            obj.transform.position = noteVacancys[position - 1].GetObj().transform.position;
            obj.transform.localScale = new Vector3(0.6f, 0.6f, 0);
        }

        // 开始游戏
        // spd: 判定线旋转速度(单位: Angle/Ms)[关于毫秒的定义请参阅MillionsecondTimer.cs]
        // bpm: 歌曲BPM
        public void _Start(float spd, int bpm)
        {
            timer = gameObject.AddComponent<BPMTimer>();
            msTimer = gameObject.AddComponent<MillisecondTimer>();
            arbiter = gameObject.AddComponent<Arbiter>();

            timer.BPM = bpm;
            arbiter.Square = Square;
            arbiter.bpmtimer = timer;

            arbiter.dbgtxt = dbg_txt.GetComponent<Text>();

            asl.Active(spd);
            DrawRing();

            for (int i = 0; i < notes.Count; i++)
            {
                Note n = notes[i];
                if (n.GetNoteType() == 1) continue;
                InitNote(n.GetObj(), n.GetNoteType(), n.GetPositionID());
                // n.GetObj().GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                if (n.GetNoteType() == 3)
                {
                    n.GetMiningObjectInside().transform.position = n.GetObj().transform.position;
                }
            }

            // 浅排个序 主要是为了优化游戏时的性能。
            // 这样遇到还未启动的音符直接跳出循环就可以了。
            notes.Sort(Cmp);
        }

        public void DrawRing()
        {
            margin_in = new GameObject("Margin_in", typeof(SpriteRenderer));
            margin_out = new GameObject("Margin_out", typeof(SpriteRenderer));
            margin_in.transform.SetParent(mainParent.transform);
            margin_out.transform.SetParent(mainParent.transform);
            margin_in.GetComponent<SpriteRenderer>().sprite = margin_out.GetComponent<SpriteRenderer>().sprite = Ring;
            margin_out.transform.localScale = new Vector3(1.75f, 1.75f, 0);
        }

        // 背对中心
        private void TransofrmRotation(GameObject obj)
        {
            Vector2 offset = obj.transform.position - Vector3.zero;
            offset = offset.normalized;
            obj.transform.up = offset;
        }

        // 添加Full音符
        // Full音符和普通音符机制和外观上都不一样，必须特殊对待(双标(不是))。
        // 好玩的: 这段代码在VSCode中的预览框里是代码最密的
        private void AddFull(int PositionID, int length, int ActiveTime, float ActiveKeep)
        {
            GameObject full_start = new("full_start", typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(NoteTab));
            GameObject full_end = new("full_end", typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(BoxCollider2D));
            full_start.GetComponent<Rigidbody2D>().gravityScale = full_end.GetComponent<Rigidbody2D>().gravityScale = 0;
            full_start.GetComponent<BoxCollider2D>().size = full_end.GetComponent<BoxCollider2D>().size = new Vector2(2f, 2f);
            full_start.GetComponent<NoteTab>().type = 1;
            InitNote(full_start, 1, PositionID);
            InitNote(full_end, 1, PositionID + length);
            TransofrmRotation(full_start);
            TransofrmRotation(full_end);
            // full_start.transform.position = new Vector3(full_start.transform.position.x, full_start.transform.position.y, -9.7f);
            notes.Add(new Note(new Full(length, full_start, full_end, FullArc, FullStart, FullEnd, ActiveTime, ActiveKeep, this), this));
        }

        // 添加音符
        // 如需添加物料, 请调用这个函数而不是new Note(int, int, int, int, int, int);
        // 注:
        // type为音符类型
        // ActiveTime为音符启动的时间, 单位为拍。关于bpm计时器请参见BPMTimer脚本
        // positionID为音符位置
        // waitTime为音符等待点击时间仅在类型为mining或licky时需要设置
        // length为full的长度, 仅在类型为full时需要设置, 以音符空位计数, 正值向右, 负值向左。
        // holdTime为按住时间, 仅mining需要设置
        // ActiveKeep为Full持续时间，仅Full需要设置
        public void AddNote(int type, int ActiveTime, int PositionID, int waitTime, int length, int holdTime, float ActiveKeep)
        {
            if ((type == 2 || type == 3) && waitTime == 0)
            {
                Debug.LogError("类型为mining或licky时waitTime参数不能为0");
                return;
            }
            if (type == 1)
            {
                if (length == 0)
                {
                    Debug.LogError("类型为full时length参数不能为0");
                    return;
                }
                AddFull(PositionID, length, ActiveTime, ActiveKeep);
                return;     // full特殊, 不能走普通note添加流程。
            }
            if (type == 3 && holdTime == 0)
            {
                Debug.LogError("类型为mining时holdTime参数不能为0");
                return;
            }
            Note note = new(type, ActiveTime, PositionID, waitTime, length, holdTime, this);
            notes.Add(note);
        }

        // 载入或更新判定数据(笨办法, 懒得动脑子())
        public void UpdateAssessData(int i = -1, Note n = null)
        {
            // 为啥必须得给局部变量设初始值啊啊啊啊啊
            NoteTab tab = default(NoteTab);
            GameObject obj = default(GameObject);
            if (i == -1)
            {
                if (n.type != 1)
                {
                    obj = n.GetObj();
                    tab = obj.GetComponent<NoteTab>();
                }

            }
            else if (n == null)
            {
                Note note = notes[i];
                if (note.type == 1)
                {
                    obj = notes[i].GetStartObj();
                }
                else obj = notes[i].GetObj();        // notes没获取到！！！
                tab = obj.GetComponent<NoteTab>();
                if (tab.type == 1) return;
            }
            else
            {
                return;
            }

            // 判定线的速度单位为度/毫秒
            // 1毫秒=1/10秒
            // 具体参考MillisecondTimer.cs
            // 所以这里的数据如下（1ms=1/1000s）：
            // perfect： ±800ms
            // good: ±300ms
            // 这是为了测试做的数据，上线的时候「一定」要改！！！！！！！！！
            tab.goodAngle = obj.transform.eulerAngles.z + asl.GetCurSpd() * 8;
            tab.goodAngle_ = obj.transform.eulerAngles.z - asl.GetCurSpd() * 8;
            tab.perfectAngle = obj.transform.eulerAngles.z + asl.GetCurSpd() * 3;
            tab.perfectAngle_ = obj.transform.eulerAngles.z - asl.GetCurSpd() * 3;
        }

        // 启动音符
        private void ActiveNote(int i)
        {
            if (notes[i].GetNoteType() == 3)
            {
                notes[i].GetMiningObjectInside().GetComponent<SpriteRenderer>().sprite = mining_in;
                notes[i].GetMiningObjectInside().transform.localScale = new Vector3(0, 0, 0);
            }

            // notes[i].GetObj().transform.localScale = new Vector2(0.75f, 0.75f);

            if (notes[i].GetNoteType() != 1)
            {
                notes[i].GetObj().transform.localScale = Vector3.zero;
                // 使note背向中心点(便于计算偏移)
                TransofrmRotation(notes[i].GetObj());
                notes[i].GetObj().AddComponent<BoxCollider2D>().size = new Vector2(2f, 2f);
            }

            UpdateAssessData(i);

            notes[i].startTime = msTimer.GetCurrentMillisecond();
            notes[i].isActive = true;
        }

        // 更新note状态
        // 这是note状态总控, 所有note状态的加载都需要这个函数完成
        // 这个函数是每帧都需要调用的
        private void UpdateNote()
        {
            for (int i = 0; i < notes.Count; i++)
            {
                // 启动音符
                if (timer.GetCurrentBPM() >= notes[i].GetActiveTime() && !notes[i].isActive)
                {
                    // Debug.Log(notes[i].type);
                    ActiveNote(i);

                    // if (notes[i].type == 1) {
                    //     StartCoroutine(arbiter.MoveStartNote(notes[i]));
                    // }
                }
                // 未到达启动时间
                // 这里因为音符已经经过排序（第一个函数，排序规则），
                // 所以一旦发现没到时间的直接return掉就行了。
                else if (timer.GetCurrentBPM() <= notes[i].GetActiveTime())
                {
                    return;
                }

                // 更新已启动音符
                if (notes[i].isActive)
                {
                    UpdateColor(i);     // 更新淡入颜色
                    if (notes[i].GetNoteType() == 2 || notes[i].GetNoteType() == 3)
                    {
                        UpdateWaitTimer(i);     // 更新等待时间                        
                    }
                }
            }
        }

        // full专属淡入
        private void UpdateFullColor(int i)
        {
            notes[i].GetObjsForFull().ForEach(n =>
            {
                SpriteRenderer sr = n.GetComponent<SpriteRenderer>();
                if (sr.color.a < 255)
                {
                    sr.color += new Color(0, 0, 0, 0.075f * Time.deltaTime * 60);

                }
            });
        }

        // 更新音符淡入
        // 我其实还想加一个非线性的放大进入
        // ⬆️已实现
        private void UpdateColor(int i)
        {
            if (notes[i].GetNoteType() == 1)
            {
                UpdateFullColor(i);
                return;
            }
            if (Mathf.Round(notes[i].GetObj().GetComponent<SpriteRenderer>().color.a) < 254)
            {
                // 这就是那个非线性的放大进入
                // 原理就是通过SmoothDamp更新音符的缩放。
                // 速度存在note对象里，所以不用单独开变量。
                notes[i].GetObj().transform.localScale = Vector3.SmoothDamp(
                    notes[i].GetObj().transform.localScale,
                    new Vector3(0.6f, 0.6f, 0),         // 音符大小就是0.6f，别犟（
                    ref notes[i].currentVeloticy,
                    0.1f
                );
                notes[i].GetObj().GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.075f * Time.deltaTime * 60);
            }
        }

        // 更新音符等待时间(仅mining/lickly)
        private void UpdateWaitTimer(int i)
        {
            // 这个if翻译成人话:
            // 当note是mining或lickly
            // 且过等待时间没点上的话
            if (
                (
                    notes[i].GetNoteType() == 2
                    || notes[i].GetNoteType() == 3
                )
                && !notes[i].GetObj().GetComponent<NoteTab>().isHold
                && !notes[i].GetObj().GetComponent<NoteTab>().isHolded
            )
            {
                // 未到达等待时间
                if (msTimer.GetCurrentMillisecond() < notes[i].startTime + notes[i].waitTime)
                {
                    return;
                }
                // 已到达就删除
                notes[i].isActive = false;
                Destroy(notes[i].GetObj());
                notes.Remove(notes[i]);
            }
        }

        // 必须在开始前做的初始化
        void Awake()
        {
            asl = gameObject.AddComponent<AssessLine>();
            uis = gameObject.AddComponent<UISystem>();
            AudioS = gameObject.AddComponent<AudioSystem>();
            asl.Square = Square;
            asl.uis = uis;
            uis.font = font;

            // icn = GetComponent<ICN>();
        }

        // 这就是[自动化]
        // 所有需要完成的就这几个部分
        void Start()
        {
            Input.multiTouchEnabled = true;        // 启用多点触控

            InitPositions();        // 初始化

            // 开场动画();
            // AddNote(0, 9, 1);       // 添加物料
            AddNote(1, 180, 3, 0, 5, 0, 1800);
            // AddNote(2, 10, 7, 30);
            // if(加载完毕){
            //   结束动画();
            //   _start();
            // }

            _Start(12, 180);        // 确认开始
        }

        // Update is called once per frame
        void Update()
        {
            // 初始化完毕, 确认启动
            if (asl.isDone) isActive = true;
            if (isActive)
            {
                UpdateNote();       // 更新音符状态
                if (!AudioS.isPlaying)
                {        // 放歌
                    AudioS.Play(Music);
                }
            }
        }
    }
}
