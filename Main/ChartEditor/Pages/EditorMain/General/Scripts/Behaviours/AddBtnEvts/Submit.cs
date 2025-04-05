using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using TMPro;

namespace ChartEditor.Main.Generic.Behaviours.AddButtonEvent
{
    // 主页的Add...按钮提交事件
    // 动画部分几乎是抄的AddBtn.cs
    // 我抄我自己（？
    public class Submit : MonoBehaviour
    {
        public GameObject AddPanel;
        private Main.Generic.Generic Generic;
        private IEnumerator ExitIe;

        public TMP_Dropdown Dropdown;

        public void onClick()
        {
            Generic = GameObject.Find("Main Camera").GetComponent<ChartEditor.Main.Generic.Generic>();
            SubmitMethod();

            ExitIe = Exit();
            StartCoroutine(ExitIe);
        }
        
        private IEnumerator Exit()
        {
            Vector3 vec  = Vector3.zero;
            Generic.DiscoverMainController();
            while (!Compare(AddPanel.transform.localPosition, new Vector3(-1.6212e-05f, -800, 0)))
            {
                AddPanel.transform.localPosition = Vector3.SmoothDamp(
                    AddPanel.transform.localPosition,
                    new Vector3(-1.6212e-05f, -800, 0),
                    ref vec,
                    0.05f
                );

                yield return null;
            }
            StopCoroutine(ExitIe);

            yield return null;
        }

        private bool Compare(Vector3 x, Vector3 y)
        {
            return (Mathf.Round(x.x) == Mathf.Round(y.x)) && (Mathf.Round(x.y) == Mathf.Round(y.y));
        }

        private void SubmitMethod()
        {
            int value = Dropdown.value;
            switch (value)
            {
                case 0:
                    Generic.AddRing();
                    break;
                case 1:
                    Generic.AddEvent();
                    break;
            }
        }
    }
}