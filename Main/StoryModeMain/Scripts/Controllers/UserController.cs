using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StoryModeMain
{
    // 玩家控制器
    // 我对玩家控制问题的一些想法写在当前目录的Note.txt里
    public class UserController : MonoBehaviour
    {
        public MainCharacterController MCC;

        private MainSystem mainSystem;
        private GameObject MainCharacter, Camera;

        private Animator animator;
        private UserEventHandler eventHandler;

        private Coroutine TouchHandlerIe;
        private Coroutine KeyboardHandlerIe;

        private Vector2 ScreenSize;

        public Text text;

        private void Start()
        {
            mainSystem = GetComponent<MainSystem>();
            MainCharacter = mainSystem.MainCharacter;
            Camera = gameObject;

            animator = MainCharacter.GetComponent<Animator>();
            eventHandler = GetComponent<UserEventHandler>();

            ScreenSize = new Vector2(Screen.width, Screen.height);

            if (Input.touchSupported) TouchHandlerIe = StartCoroutine(TouchHandler());
            else KeyboardHandlerIe = StartCoroutine(KeyboardHandler());
        }

        // 这两个函数用于控制触摸管理器的开关
        // 调用着两个函数会直接导致触摸的生效和失效
        public void ActiveUserController()
        { if (Input.touchSupported) TouchHandlerIe = StartCoroutine(TouchHandler());
            else KeyboardHandlerIe = StartCoroutine(KeyboardHandler()); }
        public void StopUserController()
        { if (Input.touchSupported) StopCoroutine(TouchHandlerIe);
            else StopCoroutine(KeyboardHandlerIe); }

        private IEnumerator TouchHandler()
        {
            while (true)
            {
                if (Input.touchCount <= 0) yield return null;

                // 打开背包的请求
                // 我想把打开背包设置为双指连点两次
                if (Input.touchCount == 2)
                {
                    HandleTouch(Input.touches);
                    yield return null;
                }

                foreach (Touch i in Input.touches)
                    HandleTouch(i);

                yield return new WaitForEndOfFrame();
            }
        }

        private void HandleTouch(Touch touch)
        {
            // 如果移开手指时点击到可交互元素
            if (touch.phase == TouchPhase.Ended)
            {
                ProcessInteract(touch);
                return;
            }

            ProcessMovement(touch);
        }

        // 用于处理物品栏请求的TouchHandler
        private void HandleTouch(Touch[] touches)
        {
            foreach (Touch touch in touches)
            {
                if (touch.tapCount < 2) return;
            }
            eventHandler.InfoPanelEvent();
        }

        // 处理交互请求
        private void ProcessInteract(Touch touch)
        {
            animator.resetAnimation();

            Ray FRay = UnityEngine.Camera.main.ScreenPointToRay(touch.position);
            Vector3 touchPos = new Vector3(FRay.origin.x, FRay.origin.y, 20);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector3.back, Mathf.Infinity);

            if (hit) CatchRay(hit);
        }

        // 处理移动请求
        private void ProcessMovement(Touch touch)
        {
            // X与Y的移动距离，基于屏幕坐标 
            // 该数值为正数代表向正方向移动，反之同理。
            float XMovementDistance = touch.position.x - touch.rawPosition.x;
            float YMovementDistance = touch.position.y - touch.rawPosition.y;

            if ((Mathf.Abs(XMovementDistance) / ScreenSize.x < 0.025f) || touch.phase == TouchPhase.Ended)
            {
                animator.resetAnimation();
                return;
            }

            // 判断是否是移动请求的方法是手指移动距离对于屏幕大小的占比
            // 目前采用大于等于10%屏幕尺寸为普通移动请求，大于等于20%为奔跑请求(再执行一遍位置更新)
            if (Mathf.Abs(XMovementDistance) / ScreenSize.x >= 0.1f)
            {
                animator.playAnimation(XMovementDistance > 0 ? 4 : 3);
                MCC.Movement(XMovementDistance > 0 ? 1 : -1);
            }
            if (Mathf.Abs(XMovementDistance) / ScreenSize.x >= 0.20f)
            {
                MCC.Movement(XMovementDistance > 0 ? 1 : -1);
            }
        }

        // 处理点击事件
        private void CatchRay(RaycastHit2D ray)
        {
            GameObject hitObject = ray.collider.gameObject;

            switch (hitObject.tag)
            {
                case "InteractiveNPC":
                    eventHandler.NPCEvent(hitObject);
                    break;
            }
        }


        // 调试用键盘控制器
        private IEnumerator KeyboardHandler()
        {
            while (true)
            {
                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    animator.resetAnimation();
                    yield return null;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    MCC.Movement(1);
                    animator.playAnimation(4);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    MCC.Movement(-1);
                    animator.playAnimation(3);
                }

                yield return null;
            }
        }
    }
}
