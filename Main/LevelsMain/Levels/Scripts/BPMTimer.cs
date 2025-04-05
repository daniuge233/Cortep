using UnityEngine;

namespace LevelsMain
{
    // BPM计时器
    public class BPMTimer : MonoBehaviour
    {
        public int BPM;
        private float CurrentBPM = 0;
        private float BPMIncrease;

        // Update is called once per frame
        void Update()
        {
            // BPM * (1/60)为了计算每秒的BPM增量
            // BPM * Time.deltaTime为了计算确切的增量/

            BPMIncrease = BPM * 0.166666666667f * Time.deltaTime;
            CurrentBPM += BPMIncrease;
        }

        public float deltaBPM()
        {
            return BPMIncrease;
        }

        public float GetCurrentBPM()
        {
            return CurrentBPM;
        }
    }
}