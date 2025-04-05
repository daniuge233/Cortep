using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomePage
{
    public class UISystem : MonoBehaviour
    {
        // public Slider SliderSlecter;        // �ײ�����

        private MainSystem mainSystem;      // MainSystem
        private CameraController cameraController;      // ���������

        // public Coroutine SliderUpdater;       // 更新Slider位置的协程函数

        // public GameObject Canvas;

        // 淡入与淡出动画
        // 适用于Image component.
        public IEnumerator FadeIn(Image img, float time) {

            float step = 1 / time * Time.deltaTime;

            while (img.color.a < 1) {
                img.color = new Color
                (img.color.r, img.color.g, img.color.b, img.color.a + step);

                yield return null;
            }
            
            yield return null;
        }

        public IEnumerator FadeOut(Image img, float time, bool doDesyroy = false) {

            float step = 1 / time * Time.deltaTime;

            while (img.color.a > 0) {
                img.color = new Color
                (img.color.r, img.color.g, img.color.b, img.color.a - step);

                yield return null;
            }

            if (doDesyroy) Destroy(img.gameObject);
            yield return null;
        }


        // Slider与相机保持间距
        // 所有需要保持间距类似的功能都可以参考这个
        // 但一定要比主动对象晚更新！
        // 或使用协程函数
        // private IEnumerator UpdateSliderPosition()
        // {
        //     while (true) 
        //     {
        //         Vector2 screenPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        //         RectTransformUtility.ScreenPointToWorldPointInRectangle(SliderSlecter.gameObject.GetComponent<RectTransform>(), screenPosition, Camera.main, out Vector3 worldPosition);
        //         worldPosition.z = transform.position.z + 9.5f;
        //         worldPosition.y -= 4.5f;
        //         SliderSlecter.transform.position = worldPosition;

        //         yield return null;
        //     }

        //     // yield return null;
        // }


        // 创建一个按钮对象
        public GameObject CreateButton(Sprite sprite, int type, int index) 
        {
            GameObject obj = new("button", typeof(RectTransform), typeof(Button));
            Image img = obj.AddComponent<Image>();
            ButtonTab tab = obj.AddComponent<ButtonTab>();
            img.sprite = sprite;
            img.color = new Color(255, 255, 255, 0);
            tab.type = type;
            tab.index = index;
            tab.mainSystem = mainSystem;
            obj.tag = "Button";

            obj.transform.SetParent(mainSystem.UI_Canvas.transform);

            StartCoroutine(FadeIn(img, 0.1f));

            return obj;
        }

        // 切换背景图片
        public IEnumerator SwitchBackground(Sprite target) 
        {
            Image img = mainSystem.BG.GetComponent<Image>();
            float step = 1 / 0.125f * Time.deltaTime;

            if (img.color.a == 0)
            {
                img.sprite = target;
                while (img.color.a < 1) 
                {
                    img.color = 
                    new Color(img.color.r, img.color.g, img.color.b, img.color.a + step);
                    yield return null;
                }
            }
            else
            {
                while (img.color.a > 0)
                {
                    img.color = 
                    new Color(img.color.r, img.color.g, img.color.b, img.color.a - step);
                    yield return null;
                }
                img.sprite = target;
                while (img.color.a < 1) 
                {
                    img.color = 
                    new Color(img.color.r, img.color.g, img.color.b, img.color.a + step);
                    yield return null;
                }
            }

            yield return null;
        }


        private void Start()
        {
            mainSystem = GetComponent<MainSystem>();
            cameraController = GetComponent<CameraController>();
            // SliderSlecter.maxValue = mainSystem.ChaptersListNT;

            // SliderUpdater = StartCoroutine(UpdateSliderPosition());
        }

        // private void Update()
        // {
        //     cameraController.Schedule = (int)SliderSlecter.value;
        // }
    }

}
