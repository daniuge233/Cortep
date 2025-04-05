using System;
using UnityEngine;

namespace LevelsMain
{
    // 判定线
    public class AssessLine : MonoBehaviour
    {

        private GameObject line;
        private bool isActive = false;
        public float targetSpd;
        private SpriteRenderer sr;
        public Sprite Square;
        private float tspd;
        private float percent = 0.05f;

        public UISystem uis;
        private MillisecondTimer mst;

        public bool isDone = false;

        private float elapsedTime;

        private ICUI UInterface;

        // private float percent = 0.0f;

        public void Active(float ts)
        {
            this.targetSpd = ts;
            isActive = true;
        }

        public GameObject GetLine()
        {
            return line;
        }

        public float GetCurSpd()
        {
            return tspd;
        }

        void Awake()
        {
            line = new GameObject("Assessline", typeof(SpriteRenderer));
            line.transform.SetParent(GetComponent<MainSystem>().mainParent.transform);
        }
        void Start()
        {
            sr = line.GetComponent<SpriteRenderer>();
            sr.sprite = Square;
            line.transform.position = new Vector3(0, 0, -5);
            line.transform.localScale = new Vector3(0.1f, 0, 0);
            line.AddComponent<BoxCollider2D>();
            line.GetComponent<BoxCollider2D>().isTrigger = true;

            mst = gameObject.GetComponent<MillisecondTimer>();
            UInterface = uis.GetComponent<ICUI>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {

                // 平滑缩放变长
                // ICUI.Smooth函数进行非线性插值, 一定要加四舍五入！！！！
                // 不然会导致始终达不到而时间延长
                // 一般保留5位小数就够了
                if (Math.Round(sr.transform.localScale.y, 5) < 9.45f)
                {
                    Vector3 cur = sr.transform.localScale;
                    Vector3 tar = new Vector3(0.1f, 9.5f, 10);
                    float spd = 0.1f;
                    //Application.targetFrameRate = 15;
                    sr.transform.localScale = UInterface.Smooth(cur, tar, spd, ref elapsedTime);

                    // 平滑加速旋转
                }
                else if (percent < 1)
                {
                    if (targetSpd > 0)
                    {
                        tspd = targetSpd * mst.deltaMillionTime() * percent;
                        percent += mst.deltaMillionTime() * 0.06f;
                    }
                    else
                    {
                        tspd = -targetSpd * mst.deltaMillionTime() * -percent;
                        percent += mst.deltaMillionTime() * 0.06f;
                    }

                }
                else
                {
                    isDone = true;
                    if (targetSpd > 0)
                    {
                        tspd = targetSpd * mst.deltaMillionTime() * 1;
                    }
                    else
                    {
                        tspd = -targetSpd * mst.deltaMillionTime() * -1;
                    }
                }
                line.transform.eulerAngles = new Vector3(0, 0, line.transform.eulerAngles.z + tspd);
            }
        }
    }
}