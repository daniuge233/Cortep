using UnityEngine;

namespace StoryModeMain
{
    public class UserEventHandler : MonoBehaviour
    {
        private MainSystem mainSystem;
        [HideInInspector] public CameraController cameraController;
        [HideInInspector] public UserController userController;
        [HideInInspector] public InfoPanelController infoPanelController;

        // 控制NPC对话状态，防止重复操作
        public bool isNPCDialoguing = false;

        // 获取两点中点
        private Vector3 Vector3Mid(Vector3 p1, Vector3 p2)
        {
            Vector3 result = new((p1.x + p2.x) * 0.5f, (p1.y + p2.y) * 0.5f, (p1.z + p2.z) * 0.5f);
            return result;
        }

        // 这两个函数用于开关用户控制器
        // 包括相机跟随和触控
        public void BlockUserControl()
        {
            cameraController.StopCameraFollower();
            userController.StopUserController();
        }
        public void UnblockUserControl()
        {
            userController.ActiveUserController();
            cameraController.ActiveCameraFollower();
        }

        // 用于开始NPC事件的函数
        public void NPCEvent(GameObject targetObject)
        {
            if (isNPCDialoguing) return;

            isNPCDialoguing = true;
            BlockUserControl();
            
            // 这里用的是测试文本，不测试的时候需要改成从文件读取对话。
            mainSystem.Chatbox.GetComponent<ChatboxController>().ShowChatbox(new string[]{"* Hi, there.\n* Can you hear me?", "* This is just a test texxxxxxxxt...", "* TestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestText..."});
            Vector3 target = Vector3Mid(transform.position, targetObject.transform.position);
            target.y = -1.75f;
            cameraController.MoveCamera(target, 0.4f);
        }

        // 用于结束NPC事件的函数
        public void FinishNPCEvent()
        {
            isNPCDialoguing = false;
            UnblockUserControl();
        }

        
        // 处理打开物品栏的请求
        public void InfoPanelEvent()
        {
            if (!infoPanelController.isShowing()) infoPanelController.ShowPanel();
            else infoPanelController.HidePanel();
        }

        private void Start()
        {
            cameraController = GetComponent<CameraController>();
            userController = GetComponent<UserController>();
            infoPanelController = GetComponent<InfoPanelController>();

            mainSystem = GetComponent<MainSystem>();
        }
    }
}