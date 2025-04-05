using System;
using System.Collections;
using UnityEngine;

namespace StoryModeMain
{
    // 相机控制器
    // 这个脚本的目的是使相机追随主角移动
    // 主角是主动对象（
    public class CameraController : MonoBehaviour
    {
        private MainSystem mainSystem;
        private GameObject MainCharacter, Camera;

        private Coroutine CameraFollowerIe;

        private void Start()
        {
            mainSystem = GetComponent<MainSystem>();
            MainCharacter = mainSystem.MainCharacter;
            Camera = gameObject;

            CameraFollowerIe = StartCoroutine(CameraFollower());
        }

        public void ActiveCameraFollower()
        { StopCameraFollower(); CameraFollowerIe = StartCoroutine(CameraFollower()); }
        public void StopCameraFollower()
        { StopCoroutine(CameraFollowerIe); }

        public void MoveCamera(Vector3 target, float time)
        { CameraFollowerIe = StartCoroutine(CameraMovement(target, time)); }

        // 普通的相机非线性运动协程
        // 这里不要和CameraFollower混着用
        // 这个在用的时候要确保CameraFollower没有启动，以免造成干扰
        private IEnumerator CameraMovement(Vector3 target, float time)
        {
            if (CameraFollowerIe != null) StopCameraFollower();

            Vector3 curVec = Vector3.zero;

            while (!mainSystem.isAchieve(gameObject.transform.position, target, 1))
            {
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, target, ref curVec, time);

                yield return null;
            }

            yield return null;
        }

        // 相机追随主角移动
        // 这个携程使用SmoothDamp会使这个过程丝滑一点（
        private IEnumerator CameraFollower()
        {
            Vector3 curVec = Vector3.zero;
            Vector3 target = default(Vector3);

            while (true)
            {
                target = MainCharacter.transform.position;
                target.y = 0;
                target.z = -10;
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, target, ref curVec, 0.45f);

                yield return null;
            }
        }
    }
}