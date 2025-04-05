using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    // 欢迎参观新版Note.cs！
    // 它也简政放权了，变成了基类
    // 几乎所有功能的实现都在各自的类里。
    public abstract class Note
    {
        public bool isActive = false;       // 标记音符是否启动
        public bool isDestroyed = false;    // 标记音符是否结束生命周期（销毁）
        public bool isHold = false;         // 标记是否被按住，用于限制Miss的判定(用于Full和Mining)

        // 这个bool解释起来可能有点麻烦。
        // 因为判定是按照判定线角度和Note旋转角度的差值和判定线旋转速度计算的
        // 所以如果不判断判定线是否已经到达过Note的话
        // 可能会出现Note刚被启动就判定为Miss的情况。
        // 「这个bool用于标记该Note是否已经过了判定区间」。
        public bool isEntered = false;

        // RinID是这个Note所在的Ring在MainSystem.Rins中的下标，index是这个Note所占用的位置。
        public int RingID;
        public int index;

        // 音符类型与定义
        public NoteType type;
        public enum NoteType
        {
            Duang,
            Mining,
            Licky,
            Full
        }

        // 在第多少拍的时候启动这个Note
        public int ActiveBeat;

        // 一个Note可能由多个GameObject组成（比如Full）
        // 这个GameObject是唯一一个用于与玩家交互的Object
        // 所有Note有关的计算全在这个Object上完成
        public GameObject MainNote;

        // 背对中心
        protected void BackToCenter(GameObject obj)
        {
            Vector2 offset = obj.transform.localPosition - Vector3.zero;
            offset = offset.normalized;
            obj.transform.up = offset;
        }

        // Note的基础构造函数
        // 必须由子类调用
        public Note(int RingID, int index, NoteType type, int ActiveBeat)
        {
            this.RingID = RingID;
            this.index = index;
            this.ActiveBeat = ActiveBeat;

            MainNote = new("MainNote", typeof(SpriteRenderer));
            MainNote.AddComponent<NoteController>().Note = this;
            MainNote.AddComponent<CircleCollider2D>().radius = 3f;
            // MainNote.AddComponent<Rigidbody2D>().gravityScale = 0;
            MainNote.transform.SetParent(MainSystem.Rings[RingID].Parent.transform);
            MainNote.transform.localScale = new Vector3(0.65f, 0.65f, 065f);
            MainNote.layer = LayerMask.NameToLayer("Notes");      // 用于触摸处理器的识别
            SpriteRenderer sr = MainNote.GetComponent<SpriteRenderer>();
            sr.color = new Color(255, 255, 255, 0);
            // sr.sortingOrder = 99;

            SetSprite(type);

            MainSystem.Rings[RingID].Notes.Add(this);
        }
        // 设置音符材质
        private void SetSprite(NoteType type)
        {
            SpriteRenderer sr = MainNote.GetComponent<SpriteRenderer>();
            switch (type)
            {
                case NoteType.Duang:
                    sr.sprite = DB.Duang;
                    break;
                case NoteType.Mining:
                    sr.sprite = DB.Mining;
                    break;
                case NoteType.Licky:
                    sr.sprite = DB.Licky;
                    break;
                case NoteType.Full:
                    sr.sprite = DB.Full;
                    break;
            }
        }

        // 获取音符旋转的角度
        // 用于计算判定线延迟
        public float GetRotation() { return MainNote.transform.eulerAngles.z; }

        // 获取Ring的函数
        public Ring GetRing() { return MainSystem.Rings[RingID]; }

        // 音符的默认启动函数
        // 不同音符的启动函数可能不一样，为了防止某些情况下需要重新写很多遍这段代码，所以用virtual
        // 不一样直接override掉就行了
        public virtual void Active()
        {
            MainNote.transform.localPosition = DB.NotePositions[index];
            BackToCenter(MainNote);
            NoteController nc = MainNote.GetComponent<NoteController>();
            nc.StartCorout(nc.FadeIn(new List<GameObject>(){MainNote}));
        }

        // 音符的默认销毁函数
        // 同上
        // 其实可以把所有音符设为MainNote的子对象
        // 这个脚本里的SetParent就是干这个的（
        // ⬆️对Full没用
        public virtual void Destroy()
        {
            // 虽然这只是代码，但我还是觉得MainNote好可怜...
            // 承担了一个Note的所有工作，
            // 结局居然是自己销毁自己...
            NoteController nc = MainNote.GetComponent<NoteController>();
            nc.destroy(MainNote);

            isDestroyed = true;
        }

        // 设置父对象的函数
        protected virtual void SetParent(List<GameObject> objs)
        {
            foreach (GameObject obj in objs)
            {
                obj.transform.SetParent(MainNote.transform);
            }
        }
        
        // 每种Note都要实现的函数
        // Note被点击的抽象函数
        public abstract void Click();
        // Note被按住的抽象函数
        public abstract void Hold();
        // Note被松开的抽象函数
        public abstract void Release();

        // Note生命周期终止的函数
        public virtual void FinishNote()
        {
            Destroy();
        }
    }
}