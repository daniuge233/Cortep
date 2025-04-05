using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 点击特效执行器
    // 负责在执行完毕后删除特效Object
    // 特效使用Unity Animation制作
    public class ClickEffectExecuter : MonoBehaviour
    {
        private void Update()
        {
            Destroy(gameObject, 0.75f);
        }
    }
}