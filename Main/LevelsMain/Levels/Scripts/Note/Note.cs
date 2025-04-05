using System.Collections.Generic;
using UnityEngine;

namespace LevelsMain
{
    // 音符的定义
    public class Note
    {
        public GameObject obj;        // 音符Object

        private GameObject obj_mining_in;        // mining里面的圆

        public List<GameObject> obj_full_arc = new List<GameObject>();     // Full的环
        public GameObject obj_start, obj_end;       // full的定位音符

        public int type = 0;                // 音符类型(0: duang   1: full   2: Licky   3: mining)
        private int ActiveTime;           // 启动时间(拍)
        public float ActiveKeep;      // 持续时间(拍)
        private int PositionID;            // 位置(根据音符空位ID)

        public bool isActive = false;

        public int waitTime;        // licky和mining的...额...等待时间(ms)?
        public int holdTime;       // mining要按住的时间(ms)
        public int startTime;         // 等待开始时间

        public int length;      // full的长度

        public bool full_isFinish = false;

        public Full full;

        public Vector3 currentVeloticy = Vector3.zero;              // 这是音符启动时的缩放动画的速度值

        // 普通构建方法
        public Note(int type, int ActiveTime, int PositionID, int waitTime, int length, int holdTime, MainSystem MainSystem)
        {
            obj = new GameObject("Note", typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(NoteTab));

            obj.transform.SetParent(MainSystem.mainParent.transform);

            if (type == 3)
            {
                obj_mining_in = new GameObject("note_mining_in", typeof(SpriteRenderer));
            }

            obj.layer = LayerMask.NameToLayer("notes");
            this.type = type;
            this.ActiveTime = ActiveTime;
            this.PositionID = PositionID;
            this.waitTime = waitTime;
            this.length = length;
            this.holdTime = holdTime;

            NoteTab nt = obj.GetComponent<NoteTab>();
            obj.GetComponent<Rigidbody2D>().gravityScale = 0;
            nt.type = type;
            nt.n = this;
        }

        // full专属构建方法
        public Note(Full full, MainSystem MainSystem)
        {
            this.full = full;
            type = 1;
            obj_start = full.StartPoint;
            obj_end = full.EndPoint;
            ActiveTime = full.ActiveTime;
            ActiveKeep = full.ActiveKeep;
            obj_full_arc = full.Arcs;
            length = full.length;
            obj_start.layer = LayerMask.NameToLayer("notes");
            obj_end.layer = LayerMask.NameToLayer("notes");

            obj_start.transform.SetParent(MainSystem.mainParent.transform);
            obj_end.transform.SetParent(MainSystem.mainParent.transform);
        }

        public GameObject GetObj()
        {
            return obj;
        }

        public GameObject GetMiningObjectInside()
        {
            return obj_mining_in;
        }

        public int GetNoteType()
        {
            return type;
        }

        public int GetActiveTime()
        {
            return ActiveTime;
        }

        public int GetPositionID()
        {
            return PositionID;
        }

        // (仅限full) 获取所有组成部分
        public List<GameObject> GetObjsForFull()
        {
            List<GameObject> objs = new List<GameObject>();
            objs.Add(obj_start);
            objs.Add(obj_end);
            obj_full_arc.ForEach(n =>
            {
                objs.Add(n);
            });
            return objs;
        }
        public List<GameObject> GetArcsForFull()
        {
            return obj_full_arc;
        }
        // (仅限full) 获取起始位置
        public GameObject GetStartObj()
        {
            return obj_start;
        }
        public GameObject GetEndObj()
        {
            return obj_end;
        }
    }
}