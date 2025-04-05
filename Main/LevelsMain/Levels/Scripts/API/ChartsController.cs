using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 这个脚本负责调度谱面API
namespace LevelsMain
{
    public class ChartsController : MonoBehaviour, ChartsAPI
    {
        private MainSystem mainSystem;

        private void Start()
        {
            mainSystem = GetComponent<MainSystem>();
        }

        // 这里获取到API调度器
        ChartAPIs ChartsAPI.GetChartController()
        {
            return new ChartAPIs(mainSystem);
        }
    }

    public class ChartAPIs
    {
        public NoteAPIs NoteAPIs;        // 关于音符的API
        public AssessLineAPIs AssessLineAPIs;

        public ChartAPIs(MainSystem mainSystem)
        {
            NoteAPIs = new NoteAPIs(mainSystem);
            AssessLineAPIs = new AssessLineAPIs(mainSystem);
        }
    }
}