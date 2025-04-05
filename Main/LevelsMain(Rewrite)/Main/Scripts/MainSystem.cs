using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LevelsMain_O
{
    // 😄欢迎参观新关卡主系统😄
    // 相较于旧的关卡主系统,这套系统更加「简政放权」(?)
    // 把大量的工作交给了其他脚本.
    // 主系统不再负责游戏内控制,
    // 只是做简单的初始化与启动工作.
    // 相当于电脑的BIOS
    // 新版游戏系统总共1304行
    public class MainSystem : MonoBehaviour
    {
        public static List<Ring> Rings = new();
        // 判定数据
        // Perfect: ±80ms, Good: ±120ms
        public static int PerfectRange = 80, GoodRange = 120;

        public static int RingCnt = 0, NotesCnt = 0;

        // 存储所有参与游戏逻辑运算的Behaviour脚本
        // 为了实现游戏暂停的功能
        // 暂停原理请参考Pause函数的注释
        public static List<Behaviour> Components = new();

        // 开始游戏的函数
        public static void Loader(string FilePath)
        {
            Level level = LevelLoader.LoadLevelFile(FilePath);
    
            InitUI();
            InitLevel(level);

            DB.BPM = 180;       // 设置BPM，这里以后要改成从文档读取
        }

        // 创建默认UI
        public static void InitUI()
        {
            Ring ring = new Ring(0);
            ring.ActiveAssessLine();
        }

        // 初始化关卡
        private static void InitLevel(Level level)
        {
            // 这里要写根据level添加Note和特效
            // 但用于测试就略过了
            // 只写测试要用的（
            // 记住要写switch判断note形式！！！

            // new Duang(0, 7, 0);

            // new Mining(0, 5, 2, 3000);
            
            new Full(0, 7, 0, -5);
            new Duang(0, 1, 0);
            new Mining(0, 6, 30, 3000);
            // new Mining(0, 8, 7, 3000);
            NotesCnt+=3;
        }

        public static void StartGame()
        {
            Timer.StartTimer();

            // 这里要改成根据level进行更新
            // 现在是测试代码
            Rings[0].GetAssessLine().GetComponent<AssessLine>().UpdateAssessLine(30);
        }

        // 暂停游戏
        // 通过暂停MonoBehaviour休眠游戏逻辑的处理
        // 它会暂停AssessLine(s)、Timer、UserController、NoteHandler的更新
        // 对应反馈、时间、交互、逻辑的暂停。
        public static void Pause()
        {
            foreach(Behaviour component in Components)
            {
                component.enabled = false;
            }
        }

        // 继续游戏
        // 与暂停同理
        public static void Continue()
        {
            foreach(Behaviour component in Components)
            {
                component.enabled = true;
            }
        }
        
        private void Awake()
        {
            Application.targetFrameRate = 9999;
        }

        private void OnEnable()
        {
            foreach(Component component in gameObject.GetComponents(typeof(Component)))
            {
                // 加一个try-catch是为了防止因非Behaviour脚本的对象无法转为Behaviour类型而导致的报错
                // 如Transform、Database等
                try
                { 
                    if (component == Camera.main) continue;
                    Components.Add((Behaviour)component);
                } 
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        // 测试脚本
        public void Start()
        {
            Loader("");
            StartGame();
        }
    }
}