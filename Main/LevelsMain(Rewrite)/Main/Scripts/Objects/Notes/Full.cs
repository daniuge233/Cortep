using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // Full(滑动音符)的定义
    // 最麻烦的音符
    // 其实这个版本的Full做起来比老版本简单多了（
    public class Full : Note
    {
        // 所有轨道组成部分
        private List<GameObject> Track_parts = new();
        private GameObject EndObject;

        private GameObject FakeParent;

        private int length;

        // 判定时使用的判定线和Note的旋转角度
        private float LineRotation, NoteRotation;
        private GameObject AssessLine;

        // 用于-90
        private int AslRotationOffset = 0;


        // 用着最精简的构造函数
        // 算着最复杂的判定机制（
        public Full(int RingID, int index, int ActiveBeat, int length) : base(RingID, index, NoteType.Full, ActiveBeat) { this.length = length; }

        // Full专属启动函数
        public override void Active()
        {
            AssessLine = GetRing().GetAssessLine();

            MainNote.transform.localPosition = DB.NotePositions[index];
            BackToCenter(MainNote);
            NoteController nc = MainNote.GetComponent<NoteController>();

            EndObject = new GameObject("Full_EndObject", typeof(SpriteRenderer));
            InitEndObject(index, length);
            InitTrack(length);

            // 很麻烦的一点，Full的MainNote需要移动
            // 为了保证MainNote不会带着整个Full动，必须弄一个伪父元素
            // 这样一来SetParent和Destroy都得override...
            FakeParent = new("Full_FakeParent");
            SetParent(Track_parts);
            SetParent(new() { EndObject });
            MainNote.transform.SetParent(FakeParent.transform);
            FakeParent.transform.SetParent(GetRing().Parent.transform);

            nc.StartCorout(nc.FadeIn(new List<GameObject>() { MainNote, EndObject }));
            nc.StartCorout(nc.FadeIn(Track_parts));
        }

        // 初始化Full终点的函数
        private void InitEndObject(int index, int length)
        {
            SpriteRenderer sr = EndObject.GetComponent<SpriteRenderer>();
            sr.sprite = DB.Full_End;
            sr.color = new Color(255, 255, 255, 0);
            sr.transform.localScale = new Vector3(0.65f, 0.65f, 1);
            int targetIndex = index + length;
            EndObject.transform.position = DB.NotePositions[targetIndex];
            BackToCenter(EndObject);
        }

        // 初始化Full轨道的函数
        private void InitTrack(int length)
        {
            for (int i = 0; i < Mathf.Abs(length); i++)
            {
                GameObject obj = new GameObject("Full_track", typeof(SpriteRenderer));
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                sr.sprite = DB.Full_track;
                sr.color = new Color(255, 255, 255, 0);

                if (length > 0) Forward(obj, i);
                if (length < 0) Backward(obj, i);

                Track_parts.Add(obj);
            }
        }

        // 分别负责长度为正、负的圆弧计算
        // length > 0 ｜ 顺时针
        // length < 0 ｜ 逆时针
        // 每个track part的圆心角为45度
        // 也就是每两个Note空位之间的圆心角度数。
        private void Forward(GameObject obj, int i)
        { obj.transform.eulerAngles = new Vector3(0, 0, MainNote.transform.eulerAngles.z - 45 * i); }
        private void Backward(GameObject obj, int i)
        { obj.transform.eulerAngles = new Vector3(0, 0, MainNote.transform.eulerAngles.z + 45 * (i + 1)); }


        // 在手指落下时进行的处理
        // 用于判断判定线方向等
        public override void Click()
        {
            isHold = true;

            // 判断提前按下
            if (result == ArbitingData.ArbitingStatus.BadMiss) FinishNote();

            LineRotation = Arbiter.EulerAngles2InspectorRotation(AssessLine.transform.up, AssessLine.transform.eulerAngles).z;
            NoteRotation = Arbiter.EulerAngles2InspectorRotation(MainNote.transform.up, MainNote.transform.eulerAngles).z;
            // 适配左右半圆的跟踪逻辑差异
            if (Mathf.Abs(LineRotation - NoteRotation) < Mathf.Abs(LineRotation - 90 - NoteRotation))
            {
                AslRotationOffset = 90;
            }
            else
            {
                AslRotationOffset = -90;
            }
        }

        public override void Hold()
        {
            // 适配Debugger（（（（
            // 因为Debugger是直接调用的Hold（（（（
            if (!isHold) Click();

            UpdateMainNote();
            UpdateLifeCycle();
            UpdateEffect();
        }

        // 提前松开
        // 这里直接用判定器的BadMiss判定系统算了
        public override void Release() 
        {
            ArbitingData.ArbitingStatus res = new ArbitingData(this, EndObject).Arbit();
            if (res == ArbitingData.ArbitingStatus.BadMiss)
            {
                result = res;
                FinishNote();
            }
        }


        // 更新MainNote
        // 使MainNote跟随判定线转动
        private void UpdateMainNote()
        {
            // 因为MainNote要时刻跟随判定线移动
            // 所以需要实时更新判定线位置
            LineRotation = Arbiter.EulerAngles2InspectorRotation(AssessLine.transform.up, AssessLine.transform.eulerAngles).z;

            // 圆弧半径是常量
            const float r = 3.1225f;

            MainNote.transform.localPosition = GetRoundPoint(Vector3.zero, r, LineRotation + AslRotationOffset);
        }

        // 计算圆上任意一点位置
        // x1 = x0 + r * cos(a0 * pi / 180) 
        // y1 = y0 + r * sin(a0 * pi / 180)
        // 圆点坐标：(x0, y0)     半径：r    角度：a0      圆周率：pi
        private Vector3 GetRoundPoint(Vector3 center, float r, float Sita, float PI = Mathf.PI)
        {
            float x = center.x + r * Mathf.Cos(Sita * PI / 180);
            float y = center.y + r * Mathf.Sin(Sita * PI / 180);

            return new Vector3(x, y, 0);
        }

        // Full生命周期更新函数
        // 听着高级，其实就是判断Full和EndPoint的位置关系（
        private void UpdateLifeCycle()
        {
            BackToCenter(MainNote);
            if (isAchieved())
            {
                FinishNote();
            }
        }
        // 这个函数可以判断是否已经到达终点。
        // 原理是通过判断旋转角度增减性确认判断方式是大于还是小于
        // 只要满足就返回true.
        private bool isAchieved()
        {
            float currentRotation = MainNote.transform.eulerAngles.z;
            float target = EndObject.transform.eulerAngles.z;
            AssessLine asl = AssessLine.GetComponent<AssessLine>();
            if (asl.GetSpeed() > 0)
            {
                if (currentRotation >= target) return true;
            }
            if (asl.GetSpeed() < 0)
            {
                if (currentRotation <= target) return true;
            }

            return false;
        }

        // 放特效调度函数
        // Full要跟着MainNote播放特效
        // 所以单独做一个播放特效的函数
        private float T = 0.75f;
        private void UpdateEffect()
        {
            T += Time.deltaTime;

            if (T >= 0.75f)
            {
                T = 0;
                NoteHandler.PlayEffect(this, result);
            }
        }


        // 专门为Full重写的SerParent和Destory函数
        // 其他Note都没这个待遇（
        protected override void SetParent(List<GameObject> objs)
        {
            foreach (GameObject obj in objs)
            {
                obj.transform.SetParent(FakeParent.transform);
            }
        }
        public override void Destroy()
        {
            NoteController nc = MainNote.GetComponent<NoteController>();
            nc.destroy(FakeParent);

            isDestroyed = true;
        }


        public override void FinishNote()
        {
            Destroy();
        }
    }
}
