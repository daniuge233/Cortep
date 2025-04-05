using System.Collections;
using UnityEngine;

namespace HomePage
{
    public class CameraShake : MonoBehaviour
    {
        public bool isActive = false;

        public float floatRange = 0.025f;        // 漂浮范围
        public float smoothSpeed = 2.5f;       // 平滑速度

        public float boundaryRadius = 10.0f;    // 限制相机浮动的半径

        private Vector3 targetPosition;        // 目标位置

        void Start()
        {
            // 初始时设置目标位置为当前相机位置
            targetPosition = transform.position;
        }

        void Update()
        {
            if (isActive)
            {
                targetPosition = GetRandomPosition();

                // 使用Vector3.Lerp逐渐过渡到目标位置
                Vector3 CurPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                transform.position = new Vector3(CurPosition.x, CurPosition.y, transform.position.z);

                // 限制相机位置在一个半径范围内
                //transform.position = Vector3.ClampMagnitude(transform.position, boundaryRadius);
            }
        }

        Vector3 GetRandomPosition()
        {
            // 生成随机的漂浮偏移
            float xOffset = Random.Range(-floatRange, floatRange);
            float yOffset = Random.Range(-floatRange, floatRange);

            // 返回新的漂浮位置
            return new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
        }
    }

}