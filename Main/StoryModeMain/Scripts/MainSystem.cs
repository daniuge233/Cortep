using System;
using System.Collections;
using UnityEngine;

namespace StoryModeMain
{
    // 故事模式RPG主系统
    public class MainSystem : MonoBehaviour
    {
        public GameObject MainCharacter, Chatbox;
        private GameObject Camera;

        public bool isAchieve(Vector3 origin, Vector3 target, int offset = 2) 
        {
            if (Math.Round(target.x, offset) == Math.Round(origin.x, offset) && 
                Math.Round(target.y, offset) == Math.Round(origin.y, offset) && 
                Math.Round(target.z, offset) == Math.Round(origin.z, offset))
            {

                return true;
            }

            return false;
        }

        private void Awake()
        {
            Application.targetFrameRate = 114514;
            Camera = gameObject;
        }
    }
}