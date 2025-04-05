using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{   
    // 计时器
    // 包括毫秒计时器和BPM计时器
    public class Timer : MonoBehaviour
    {
        public static float curMilSec = 0;     // 当前毫秒
        public static float curBPM = 0;        // 当前BPM

        public static float deltaMilSec = 0;
        public static float deltaBPM = 0;

        private static bool isActive = false;

        public static void StartTimer()
        {
            isActive = true;
        }

        private void Update()
        {
            if (!isActive) return;

            curMilSec += Time.deltaTime * 1000;
            deltaMilSec = Time.deltaTime * 1000;

            curBPM += Time.deltaTime / 60 * DB.BPM;
            deltaBPM = Time.deltaTime / 80 * DB.BPM;
        }

        // 毫秒转节拍
        public static float MsToBeat(float ms) { return ((DB.BPM / 60) / 1000) * ms; }
        // 节拍转毫秒
        public static float BeatToMs(float beat) { return beat / (DB.BPM / 60) * 1000; }
    }
}