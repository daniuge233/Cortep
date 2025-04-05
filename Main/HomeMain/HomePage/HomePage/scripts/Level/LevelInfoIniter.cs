using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 这个脚本用于初始化Level显示区
// 不继承MonoBehaviour好像没发用AlignmentTypes
// 单开一个脚本用于初始化也能减少代码量
namespace HomePage
{
    public class LevelInfoIniter : MonoBehaviour
    {
        private void Start() 
        {
            TextMeshPro tmp = GetComponent<TextMeshPro>();
            GameObject mainCamera = GameObject.Find("Main Camera");
            MainSystem mainSystem = mainCamera.GetComponent<MainSystem>();

            // 傻逼留着弃用的用户手册不删http://digitalnativestudios.com/textmeshpro/docs/ScriptReference/AlignmentTypes.html
            // 气死我了！！！！
            // 为什么不删老API！！！！！
            // 以后所有TextMeshPro有关的脚本参考只用https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/api/
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.font = mainSystem.font;

            if (gameObject.name == "Title") 
            {
                tmp.fontSize = 6f;
                gameObject.transform.localPosition = new Vector3(0, 0.9f, 0);
            }
            else if (gameObject.name == "Author")
            {
                tmp.fontSize = 4f;
                tmp.color = new Color(110, 110, 110);
                gameObject.transform.localPosition = new Vector3(0, 0.45f, 0);
            }
            // 等级的格式应为：[EZ]  [HD]  [IN]  [AT]
            // 难度用数字表示，每个难度间两个空格
            else if (gameObject.name == "Level") 
            {
                Chapter chapter = mainSystem.chapters
                [   
                    // 这一长串其实就为了获取到ChapterID
                    // 先找到父元素，再找到外圈元素上的ChapterTab
                    // 最后获取ChapterID.
                    GetComponentInParent(typeof(SpriteRenderer))
                    .GetComponentInChildren<ChapterTab>()
                    .ChapterIndex
                ];

                gameObject.AddComponent<BoxCollider>().size = new Vector3(2.5f, 1, 1);
                gameObject.AddComponent<LevelSwitcher>().chapter = chapter;
                gameObject.tag = "LevelSwitch";
                tmp.fontSize = 4f;
                gameObject.transform.localPosition = new Vector3(0, -1.25f, 0);
            }
            
            // 「他自杀了」
            // (doge)
            Destroy(this);
        }
        
    }
}
