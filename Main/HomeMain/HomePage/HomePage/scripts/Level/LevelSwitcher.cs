using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 该脚本用于每个章节下每个关卡切换难度
namespace HomePage
{
    public class LevelSwitcher : MonoBehaviour
    {
        public int curLevel = 0;
        private TextMeshPro tmp;

        public Chapter chapter;

        public void SwitchLevel() 
        {
            Level sel = chapter.GetLevelsList()[chapter.mainSystem.GetComponent<LevelHandler>().GetCurLevel()];

            sel.CurrentSelectedDifficulty++;
            if (sel.CurrentSelectedDifficulty > 3)
            {
                sel.CurrentSelectedDifficulty = 0;
            }

            UpdateText(sel);
        }

        // 更新Text
        public void UpdateText(Level sel)
        {
            string res = "";
            for (int i = 0; i < sel.Difficulties.Count; i++)
            {
                if (i == sel.CurrentSelectedDifficulty) 
                {
                    res += "<color=black>" + sel.Difficulties[i].ToString() + "</color>  ";
                }
                else
                {
                    res += sel.Difficulties[i].ToString() + "  ";
                }
            }

            tmp.text = res;
        }

        private void Start() 
        {
            tmp = gameObject.GetComponent<TextMeshPro>();
        } 
    }
}