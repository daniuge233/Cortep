using System.Collections.Generic;
using UnityEngine;

namespace LevelsMain
{
    // full音符的定义

    // 寄语谱师: 请不要在内圆设置Full或将开始或结束的Note放在内圆, 这会导致重大Bug.
    // 内圆本来空间也不够放Full, 不值当的
    public class Full
    {
        public int length;
        public GameObject StartPoint;
        public GameObject EndPoint;
        public List<GameObject> Arcs = new();
        public Sprite sprite_arc;
        public Sprite sprite_start;
        public Sprite sprite_end;
        public int ActiveTime;
        public float ActiveKeep;

        // 其他的都没什么不同，ActiveKeep为主动移动的时间，单位为拍
        public Full(int length, GameObject StartPoint, GameObject EndPoint, Sprite sprite_arc, Sprite sprite_start, Sprite sprite_end, int ActiveTime, float ActiveKeep, MainSystem MainSystem)
        {
            this.length = length;
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;
            this.sprite_arc = sprite_arc;
            this.sprite_start = sprite_start;
            this.sprite_end = sprite_end;
            this.ActiveTime = ActiveTime;
            this.ActiveKeep = ActiveKeep;

            EndPoint.GetComponent<SpriteRenderer>().sprite = sprite_end;
            StartPoint.GetComponent<SpriteRenderer>().sprite = sprite_start;

            StartPoint.transform.position = new Vector3(StartPoint.transform.position.x, StartPoint.transform.position.y, -1f);
            EndPoint.transform.position = new Vector3(EndPoint.transform.position.x, EndPoint.transform.position.y, -1f);
            //StartPoint.AddComponent<FullStartPointCollider>();

            //FullStartPointCollider FSPC = StartPoint.GetComponent<FullStartPointCollider>();
            //FSPC.EndPoint = EndPoint;

            CreateArc(length, MainSystem);
        }

        // 实现反方向full
        // 原理就是用正方向full的方式从结束的note那里开始搞
        private void CreateArc_l(int length, MainSystem MainSystem)
        {
            for (int i = 0; i > length; i--)
            {
                GameObject obj = new("Full_Arc", typeof(SpriteRenderer));
                obj.GetComponent<SpriteRenderer>().sprite = sprite_arc;
                obj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                obj.transform.SetParent(MainSystem.mainParent.transform);
                obj.transform.position = Vector3.zero;
                float trz = EndPoint.transform.eulerAngles.z + i * 45;
                trz -= 180;
                obj.transform.eulerAngles = new Vector3(0, 0, trz);
                obj.transform.position = new Vector3(0, 0, 0.01f * i);
                Arcs.Add(obj);
            }
        }

        private void CreateArc(int length, MainSystem MainSystem)
        {
            if (length < 0)
            {
                CreateArc_l(length, MainSystem);
                return;
            }
            for (int i = 0; i < length; i++)
            {
                GameObject obj = new("Full_Arc", typeof(SpriteRenderer));
                obj.GetComponent<SpriteRenderer>().sprite = sprite_arc;
                obj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                obj.transform.SetParent(MainSystem.mainParent.transform);
                obj.transform.position = Vector3.zero;
                // 应该是36 * i, 但为了中间无空隙使用35 * i.(此条注释暂时废弃)
                float trz = StartPoint.transform.eulerAngles.z + i * -45;
                obj.transform.eulerAngles = new Vector3(0, 0, trz);
                // 分层渲染(否则可能会闪烁好像?)
                obj.transform.position = new Vector3(0, 0, 0.01f * i);
                Arcs.Add(obj);
            }
        }
    }

    // 这个其实就是Full的碰撞器
    //public class FullStartPointCollider : MonoBehaviour
    //{
    //    public bool Start = false;
    //    public GameObject EndPoint;

    //    private void OnTriggerEnter2D(Collider2D other)
    //    {
    //        if (other.gameObject == EndPoint)
    //        {
    //            Start = true;
    //        }
    //    }
    //}
}