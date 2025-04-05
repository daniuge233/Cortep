using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChartEditor.Main.Generic.Behaviours
{
    // 主页的Add...按钮事件
    public class AddBtn : MonoBehaviour
    {
        public GameObject AddPanel;
        private GameObject MainCamera;
        private IEnumerator EnterIe;

        public void onClick()
        {
            MainCamera = GameObject.Find("Main Camera");
            EnterIe = Enter();
            StartCoroutine(EnterIe);
        }

        private IEnumerator Enter()
        {
            Vector3 vec  = Vector3.zero;
            MainCamera.GetComponent<ChartEditor.Main.Generic.Generic>().CoverMainController();
            while (!Compare(AddPanel.transform.localPosition, new Vector3(-1.6212e-05f, 0, 0)))
            {
                AddPanel.transform.localPosition = Vector3.SmoothDamp(
                    AddPanel.transform.localPosition,
                    new Vector3(-1.6212e-05f, 0, 0),
                    ref vec,
                    0.05f
                );

                yield return null;
            }
            // Debug.Log("stop");
            StopCoroutine(EnterIe);

            yield return null;
        }

        private bool Compare(Vector3 x, Vector3 y)
        {
            return (Mathf.Round(x.x) == Mathf.Round(y.x)) && (Mathf.Round(x.y) == Mathf.Round(y.y));
        }
    }
}
