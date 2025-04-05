using UnityEngine;

namespace LevelsMain
{
    // 毫秒计时器
    // 1毫秒 = 1/10秒
    public class MillisecondTimer : MonoBehaviour
    {
        private float millisecond = 0;
        private float curTime = 0.0f;

        public int GetCurrentMillisecond()
        {
            return (int)millisecond;
        }

        // 和Time.deltaTime一个意思, 就是变成了毫秒
        public float deltaMillionTime()
        {
            return Time.deltaTime * 10;
        }

        // Update is called once per frame
        void Update()
        {
            curTime += Time.deltaTime;
            millisecond = curTime * 10;

            // Debug.Log(GetCurrentMillisecond());
        }
    }
}