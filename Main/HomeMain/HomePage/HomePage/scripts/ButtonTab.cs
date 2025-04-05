using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomePage
{
    public class ButtonTab : MonoBehaviour
    {
        public MainSystem mainSystem;

        public int type;        // 0:返回按钮 1:难度切换
        public int index;       // 数据传递，可以是任何数据，但要区分调用

        private Button button;

        private void Start() 
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(onClick);
        }

        // 点击事件调度
        public void onClick() {
            if (type == 0) 
            {
                ReturnBtn();
            }
        }

        // 调度后的返回事件算法
        private void ReturnBtn() 
        {
            if (mainSystem.PlayerStatus == 1)
            {
                mainSystem.ButtonEvent(gameObject);
            }
        }
    }
}