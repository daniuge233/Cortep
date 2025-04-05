using System.Collections;
using UnityEngine;

namespace LoadMain
{
    public class MainSystem : MonoBehaviour
    {
        public GameObject
            Triangle,
            Title,
            Author,
            Chart,
            Illustration,
            Loading,
            AssessLine,
            Items;

        public Sprite triangle;
        private int curItemCnt = 0;
        public GameObject[] Items_;

        private Vector3
            Triangle_spd = Vector3.zero,
            Title_spd = Vector3.zero,
            Author_spd = Vector3.zero,
            Chart_spd = Vector3.zero,
            Illustration_spd = Vector3.zero,
            Loading_spd = Vector3.zero;

        private int[] Items_Y = {
            150, 100, 50, 0, -50, -100
        };


        private void __init__(ref GameObject obj, Vector3 pos)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = pos;
        }

        private int GetIndex()
        {
            int index = UnityEngine.Random.Range(0, 5);

            while (Items_Y[index] <= -120)
            {
                index = UnityEngine.Random.Range(0, 5);
            }

            return index;
        }

        private void __init_Items__()
        {
            int range = UnityEngine.Random.Range(4, 6);

            for (int i = 0; i < range; i++)
            {
                // 设置基本的参数
                GameObject obj = new GameObject("Item", typeof(RectTransform), typeof(SpriteRenderer));
                RectTransform rect = obj.GetComponent<RectTransform>();
                obj.transform.SetParent(Items.transform);
                float scale = UnityEngine.Random.Range(25, 45);
                obj.transform.localScale = new Vector3(scale, scale, 0);
                obj.GetComponent<SpriteRenderer>().sprite = triangle;
                obj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.8f);

                //Rigidbody2D rigidbody2D = obj.AddComponent<Rigidbody2D>();
                //rigidbody2D.gravityScale = 0;

                int index = GetIndex();

                rect.anchoredPosition = new Vector3(
                    700f,
                    Items_Y[index],
                    0
                );

                Items_Y[index] = -1000;
                Items_[curItemCnt++] = obj;

                StartCoroutine(
                    _Item(
                        Items_[curItemCnt - 1],
                        UnityEngine.Random.Range(2, 5),                 // speed
                        UnityEngine.Random.Range(5, 20),                // max
                        UnityEngine.Random.Range(15f, 55f),             // rotation
                        UnityEngine.Random.Range(0.25f, 0.75f)          // tr
                    )
                );
            }
        }

        private void init()
        {
            __init__(ref Triangle, new Vector3(-1548, 230, 100));
            __init__(ref Title, new Vector3(-900, 7.9f, 0));
            __init__(ref Author, new Vector3(-900, -55.5f, 0));
            __init__(ref Chart, new Vector3(-900, -11.1f, 0));
            __init__(ref Illustration, new Vector3(-900, -35.1f, 0));
            __init__(ref Loading, new Vector3(200, 0, 0));

            __init_Items__();
        }

        private void trans(ref GameObject obj, float x, float time, ref Vector3 spd)
        {
            RectTransform RectTransform = obj.GetComponent<RectTransform>();
            Vector2 StartPos = RectTransform.anchoredPosition;
            Vector2 TargetPos = new Vector3(x, StartPos.y);
            RectTransform.anchoredPosition = Vector3.SmoothDamp(StartPos, TargetPos, ref spd, time);
        }

        // 一个用于控制对象以Sin函数上下浮动并旋转的方法。
        // 使对象的y坐标随Sin函数而变化，同时旋转rotation度。
        // 用于加载屏幕右侧的视觉补充三角形。
        // 调用：StartCoroutine(_Item(Items[n], 5f, 45, 2.5f));
        // speed: sin函数的自增速度
        // max: 最大移动偏移值
        // rotation: 旋转角
        // tr: 飞入时间
        private IEnumerator _Item(GameObject obj, float speed, float max, float rotation, float tr)
        {
            // 物体的RectTransform
            RectTransform ObjRectTransform = obj.GetComponent<RectTransform>();
            // 物体起始坐标
            Vector3 obj_startPos = ObjRectTransform.anchoredPosition;
            // 用于Sin计算的current值
            float current = 0f;

            // 用于横向平移的x目标值
            float tarX = UnityEngine.Random.Range(225, 326);
            // 横向平移速度
            Vector3 vec = Vector3.zero;

            while (true)
            {
                ObjRectTransform.anchoredPosition = Vector3.SmoothDamp(
                    ObjRectTransform.anchoredPosition,
                    new Vector3(tarX, ObjRectTransform.anchoredPosition.y, 0),
                    ref vec,
                    tr
                );

                // current自增
                current += speed * Time.deltaTime;
                //float Pi = Mathf.PI;
                float s = Mathf.Sin(current);
                float t = s * max;
                ObjRectTransform.anchoredPosition = new Vector3(ObjRectTransform.anchoredPosition.x, obj_startPos.y + t, 0);
                obj.transform.eulerAngles = new Vector3(
                    0, 0,
                    obj.transform.eulerAngles.z + rotation * Time.deltaTime
                );

                yield return null;
            }
        }

        public void Start()
        {
            init();
        }

        public void Update()
        {
            trans(ref Triangle, -648, 0.3f, ref Triangle_spd);
            trans(ref Title, 0, 0.35f, ref Title_spd);
            trans(ref Author, 0, 0.4f, ref Author_spd);
            trans(ref Chart, 0, 0.45f, ref Chart_spd);
            trans(ref Illustration, 0, 0.5f, ref Illustration_spd);
            trans(ref Loading, 0, 0.5f, ref Loading_spd);

            AssessLine.transform.eulerAngles = new Vector3(0, 0, AssessLine.transform.eulerAngles.z - 100 * Time.deltaTime);
        }
    }
}
