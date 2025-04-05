using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    // 数据库脚本
    // 放在这里的数据会以Static形式存放在DB类中
    // 我很服气为什么Unity不把static变量序列化!
    public class Database : MonoBehaviour
    {
        [Header("Ring")]
        public Sprite Ring;

        [Header("Square")]
        public Sprite Square;

        [Header("Notes")]
        public Sprite Duang;
        public Sprite Licky;
        public Sprite Mining;
        public Sprite Mining_Inside;
        public Sprite Full;
        public Sprite Full_track;
        public Sprite Full_End;

        [Header("Debug")]
        public Text DebugText;

        [Header("ArbitData")]
        public int Perfect;
        public int Good;

        [Header("Effects")]
        [Header("传入效果对象预制件")]
        public GameObject EffectObject;

        private void Awake()
        {
            DB.Ring = Ring;
            DB.Square = Square;
            DB.Duang = Duang;
            DB.Licky = Licky;
            DB.Mining = Mining;
            DB.Mining_Inside = Mining_Inside;
            DB.Full = Full;
            DB.Full_track = Full_track;
            DB.Full_End = Full_End;
            
            DB.DebugText = DebugText;

            DB.Perfect = Perfect;
            DB.Good = Good;

            DB.EffectObject = EffectObject;
        }
    }

    public class DB
    {
        public static Sprite 
            Ring, Square, Duang, Licky, Mining, Mining_Inside, Full, Full_track, Full_End;

        public static int 
            BPM;

        public static Vector3[] NotePositions = new Vector3[16] {
            new Vector3(0, 3.2f, 0), new Vector3(2.2f, 2.2f, 0),
            new Vector3(3.2f, 0, 0), new Vector3(2.2f, -2.2f, 0),
            new Vector3(0, -3.2f, 0), new Vector3(-2.2f, -2.2f, 0),
            new Vector3(-3.2f, 0, 0), new Vector3(-2.2f, 2.2f, 0),

            new Vector3(0, 1.5f, 0), new Vector3(1.1f, 1.1f, 0),
            new Vector3(1.5f, 0, 0), new Vector3(1.1f, -1.1f, 0),
            new Vector3(0, -1.5f, 0), new Vector3(-1.1f, -1.1f, 0),
            new Vector3(-1.5f, 0, 0), new Vector3(-1.1f, 1.1f, 0),
        };

        public static Text DebugText;

        public static int Perfect;
        public static int Good;

        public static GameObject EffectObject;
    }
}