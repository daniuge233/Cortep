using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomePage
{
    public class FPS : MonoBehaviour
    {
        private float m_lastUpdateShowTime = 0f;
        
        private readonly float m_updateTime = 0.05f;
        
        private int m_frames = 0;
        
        private float m_frameDeltaTime = 0;
        private float m_FPS = 0;
        private Rect m_fps, m_dtime, m_dstat;
        private GUIStyle m_style = new GUIStyle();

        void Awake()
        {
            Application.targetFrameRate = 100;
        }

        void Start()
        {
            m_lastUpdateShowTime = Time.realtimeSinceStartup;
            m_fps = new Rect(0, 0, 100, 100);
            m_dtime = new Rect(0, 100, 100, 100);

            m_dstat = new Rect(0, 200, 100, 100);

            m_style.fontSize = 50;
            m_style.normal.textColor = Color.red;
        }

        void Update()
        {
            m_frames++;
            if (Time.realtimeSinceStartup - m_lastUpdateShowTime >= m_updateTime)
            {
                m_FPS = m_frames / (Time.realtimeSinceStartup - m_lastUpdateShowTime);
                m_frameDeltaTime = (Time.realtimeSinceStartup - m_lastUpdateShowTime) / m_frames;
                m_frames = 0;
                m_lastUpdateShowTime = Time.realtimeSinceStartup;
                
            }
        }

        void OnGUI()
        {
            GUI.Label(m_fps, "FPS: " + m_FPS, m_style);
            GUI.Label(m_dtime, "间隔: " + m_frameDeltaTime, m_style);

            GUI.Label(m_dstat, "Status: " + GetComponent<MainSystem>().PlayerStatus, m_style);
        }
    }
}
