using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

namespace ChartEditor.Main.Generic.Init
{
    public class Timeline : MonoBehaviour
    {
        public GameObject TimelineContent;

        private void Start()
        {
            int fixedBeats = 0;
            try
            {
                int fixedBPM = String2Int(Database.Database.EditingProject.BPM);
                float Beats = Generic.MusicClip.length / 60 * fixedBPM;
                fixedBeats = (int) Mathf.Ceil(Beats);
            } catch (Exception e) {
                Debug.LogError("出现了一个错误, 你的音乐文件没有正确加载。如果你不是ChartEditor的开发人员, 请检查项目文件中的音乐路径是否指向正确的音乐文件。\n" + e);
            }

            for (int i = 0; i < fixedBeats; i++)
            {
                SetTimeline(i); 
            }
        }

        private void SetTimeline(int i)
        {
            GameObject obj = new("Timeline_item", typeof(RectTransform), typeof(TextMeshProUGUI));
            TMP_Text tmpt = obj.GetComponent<TMP_Text>();
            RectTransform rt = obj.GetComponent<RectTransform>();
            tmpt.text = i.ToString();
            tmpt.alignment = TextAlignmentOptions.Center;
            tmpt.fontSize = 15;
            rt.sizeDelta = new Vector2(50, 15);

            obj.transform.SetParent(TimelineContent.transform);
            obj.transform.position = new Vector3(50 * (i + 1), 25, 0);
        }

        // string转int的函数
        // 算法基础，基操勿6（bs
        private int String2Int(string str)
        {
            int result = 0, p = 1;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                result += (str[i] - 48) * p;
                p *= 10;
            }

            return result;
        }
    }
}
