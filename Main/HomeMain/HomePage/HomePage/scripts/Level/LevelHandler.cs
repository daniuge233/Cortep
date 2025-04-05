using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 该脚本负责「全局」控制Levels
// 也就是说，跨越Chapter界限控制「所有」Levels
// 所以必须用全局索引
// 也就是mainSystem.chapters[CurrentSelectedParentID].levels[CurrentSelectedLevelID];
namespace HomePage
{
    public class LevelHandler : MonoBehaviour
    {
        private MainSystem mainSystem;

        private int CurrentSelectedChapterID = -1, CurrentSelectedLevelID = -1;      // 选择的Level的全局索引，-1为未选择

        private void Start() { mainSystem = GetComponent<MainSystem>(); }

        public void SelectLevel(Level level)
        {
            Chapter chapter = mainSystem.chapters[level.FatherChapterID];

            // 原先已有选择
            if (CanDeSelect())
            {
                DeSelect();
            }

            level.parent.tag = "Level_selected";

            chapter.SelectLevel(level.ID);

            CurrentSelectedLevelID = level.ID;
            CurrentSelectedChapterID = level.FatherChapterID;

            chapter.GetLevelSwitcher().UpdateText(chapter.GetLevelsList()[CurrentSelectedLevelID]);
        }

        // 取消当前选择
        public void DeSelect()
        {
            Chapter chapter = mainSystem.chapters[CurrentSelectedChapterID];
            Level level = chapter.GetLevelsList()[CurrentSelectedLevelID];
            
            chapter.ClearLevelInfo();

            StopCoroutine(level.AnimIe);
            level.AnimIe = StartCoroutine(chapter.LevelAnimation(level, new Vector3(0, 0, -0.0001f)));
            level.parent.tag = "Level";

            CurrentSelectedChapterID = CurrentSelectedLevelID = -1;
        }

        public int GetCurChapter() { return CurrentSelectedChapterID; }
        public int GetCurLevel() { return CurrentSelectedLevelID; }

        public bool CanDeSelect() { return (CurrentSelectedChapterID != -1 && CurrentSelectedLevelID != -1); }
    }
}