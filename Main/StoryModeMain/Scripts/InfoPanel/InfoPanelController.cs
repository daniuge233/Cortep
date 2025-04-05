using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor;
using System;
using System.Collections;

using Props;

namespace StoryModeMain
{
    public class InfoPanelController : MonoBehaviour
    {
        public MainSystem mainSystem;

        public MainCharacterController MCC;
        public UserEventHandler UEH;

        public PostProcessVolume PPV;
        public Database DB;

        [Space]
        [Header("信息栏主Object")]
        public GameObject InfoPanel;

        [Space]
        [Header("信息栏首页")]
        public GameObject MainPage;

        [Space]
        [Header("背包页面")]
        public GameObject Inventory;

        private bool PanelShowing = false;
        private Coroutine movement;

        private GameObject current;     // 当前选中

        // 所有页面
        public enum Page
        {
            MainPage,
            Inventory
        }

        // 显示角色信息面板
        public void ShowPanel()
        {
            if (PanelShowing) return;
            PanelShowing = true;

            // 启用全局效果
            // 就是背景虚化，懒得搞shader了
            // shader坑死人（（
            PPV.isGlobal = true;
            UEH.BlockUserControl();

            if (movement != null) StopCoroutine(movement);
            movement = StartCoroutine(InfoPanelMovementIE(new Vector3(0, 0, 0)));

            SwitchPage(Page.MainPage);
        }

        // 隐藏角色信息面板
        public void HidePanel()
        {
            if (!PanelShowing) return;
            PanelShowing = false;

            current.SetActive(false);

            // 同上
            PPV.isGlobal = false;
            UEH.UnblockUserControl();

            if (movement != null) StopCoroutine(movement);
            movement = StartCoroutine(InfoPanelMovementIE(new Vector3(0, -400, 0)));
        }

        // 切换页面的函数
        public void SwitchPage(Page page)
        {
            if (!PanelShowing) return;
            switch (page)
            {
                case Page.MainPage:
                    ShowPage(MainPage);
                    break;
                case Page.Inventory:
                    ShowPage(Inventory);
                    break;
            }
        }

        private void ShowPage(GameObject parent)
        {
            if (current != null) current.SetActive(false);
            current = parent;
            parent.SetActive(true);
        }

        private IEnumerator InfoPanelMovementIE(Vector3 target)
        {
            Vector3 curVec = Vector3.zero;

            while (!mainSystem.isAchieve(InfoPanel.transform.position, target, 1))
            {
                InfoPanel.transform.localPosition = Vector3.SmoothDamp(InfoPanel.transform.localPosition, target, ref curVec, 0.1f);

                yield return null;
            }

            yield return null;
        }

        public bool isShowing() { return PanelShowing; }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ShowPanel();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                HidePanel();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                SwitchPage(Page.Inventory);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                SwitchPage(Page.MainPage);
            }
        }
    }
}