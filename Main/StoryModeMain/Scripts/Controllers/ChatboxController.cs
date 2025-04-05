using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoryModeMain
{
    public class ChatboxController : MonoBehaviour
    {
        public MainSystem mainSystem;
        // public Button button;
        public TextMeshProUGUI TMP;
        [Space]
        public Typer Typer;

        private readonly Vector3 Shown = new(0, -125, 0), Hidden = new(0, -400, 0);
        private bool ShowingStatus = false;
        private Coroutine ChatboxMovement;

        public UserEventHandler EventHandler;

        // 显示聊天框
        public void ShowChatbox(string[] messages)
        {
            if (ShowingStatus) return;
            ShowingStatus = true;

            if (ChatboxMovement != null) StopCoroutine(ChatboxMovement);
            ChatboxMovement = StartCoroutine(MoveChatbox(true));
            Typer.startPrint(0.035f, messages, TMP);
        }

        // 隐藏聊天框
        public void HideChatbox()
        {
            if (!ShowingStatus) return;
            ShowingStatus = false;

            if (ChatboxMovement != null) StopCoroutine(ChatboxMovement);
            ChatboxMovement = StartCoroutine(MoveChatbox(false));
        }

        // 控制Chatbox显示和隐藏的协程函数
        // type: true为显示，false为隐藏
        private IEnumerator MoveChatbox(bool type)
        {
            Vector3 curVec = Vector3.zero;

            while (!mainSystem.isAchieve(transform.localPosition, type ? Shown : Hidden, 1))
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, type ? Shown : Hidden, ref curVec, 0.25f);

                yield return null;
            }
            StopCoroutine(ChatboxMovement);

            yield return null;
        }

        public void TyperContinue()
        {
            Typer.TyperContinue();
            if (Typer.Finish) 
            {
                HideChatbox();
                EventHandler.FinishNPCEvent();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                HideChatbox();
            }
        }
    }
}