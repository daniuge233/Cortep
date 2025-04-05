using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    // 圆环的定义
    // 因为允许存在多个圆环，所以这里要把Ring单独定义
    public class Ring
    {
        public int RingID;

        public GameObject Parent;
        
        // 组成一个Ring的三部分：内圆、外圆、判定线
        private GameObject Ring_o;
        private GameObject Ring_i;
        private GameObject AssessLine;

        public List<Note> Notes = new List<Note>();

        public Ring(int RingID)
        {
            this.RingID = RingID;

            Parent = new("ring_parent");

            Ring_o = new("Ring_Outside", typeof(SpriteRenderer), typeof(CircleCollider2D));
            Ring_i = new("Ring_Inside", typeof(SpriteRenderer));
            AssessLine = new("AssessLine", typeof(SpriteRenderer), typeof(AssessLine));
            AssessLine.transform.localPosition = new Vector3(0, 0, -1);

            Ring_o.GetComponent<CircleCollider2D>().isTrigger = true;

            SetParent(Parent, new(){Ring_o, Ring_i, AssessLine});
            // Parent.AddComponent<Arbiter>();
            InitStyle(Ring_i, Ring_o, AssessLine);

            MainSystem.Rings.Add(this);
            MainSystem.Components.Add(GetAssessLineComponent());
            MainSystem.RingCnt++;
        }

        public void ActiveAssessLine()
        {
            AssessLine.GetComponent<AssessLine>().ActiveAssessLine();
        }

        private void SetParent(GameObject parent, List<GameObject> objs)
        {
            foreach (GameObject obj in objs)
            {
                obj.transform.SetParent(parent.transform);
            }
        }
        private void InitStyle(GameObject Ring_i, GameObject Ring_o, GameObject AssessLine)
        {
            Ring_o.GetComponent<SpriteRenderer>().sprite = Ring_i.GetComponent<SpriteRenderer>().sprite = DB.Ring;
            AssessLine.GetComponent<SpriteRenderer>().sprite = DB.Square;
            Ring_o.transform.localScale = new Vector3(1.75f, 1.75f, 1);
            AssessLine.transform.localScale = new Vector3(0.075f, 9.5f, 1);
        }

        public GameObject GetAssessLine() { return AssessLine; }
        public AssessLine GetAssessLineComponent() { return AssessLine.GetComponent<AssessLine>(); }
    }
}