using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 这个脚本主要负责控制粒子发生器随相机移动
// 这样可以显著降低需要的粒子量（20000左右降到750左右）
// 以节约性能。
namespace HomePage
{
    public class ParticleController : MonoBehaviour
    {
        public GameObject ParticleHandler;
        private GameObject mainCamera;

        private void Start() 
        {
            mainCamera = gameObject;
        }

        private void LateUpdate() 
        {
            ParticleHandler.transform.position = mainCamera.transform.position;
        }
    }
}