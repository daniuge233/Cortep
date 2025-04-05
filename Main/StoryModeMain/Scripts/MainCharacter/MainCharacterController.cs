using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

using Props;

// 关于角色的所有控制功能
// 其中包括对信息面板的控制
namespace StoryModeMain
{
    public class MainCharacterController : MonoBehaviour
    {
        public GameObject MainCharacter;

        // 角色移动函数
        // dir正为向右，负为向左
        public void Movement(int dir)
        {
            Vector3 cur = MainCharacter.transform.position;
            cur.x += dir * 2f * Time.deltaTime;
            MainCharacter.transform.position = cur;
        }
    }
}