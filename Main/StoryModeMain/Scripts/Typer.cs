using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoryModeMain
{
    // 英语水平欠佳很多变量命名不规范见谅啊啊啊啊啊啊啊啊啊啊
    // 打字机脚本，可以实现大部分视觉小说中角色间对话时文字逐个出现的效果。
    public class Typer : MonoBehaviour
    {
        private TextMeshProUGUI txt;                          // text组件
        private int currentPos;                 // 当前打字位置
        private int length;                        // 内容数组长度
        private int nowLength;                 // 当前到达数组位置
        private float spd;                         // 速度
        private float timer;                     // 计时器
        private bool isActive;                  // 是否允许打字
        private bool isWaiting = false;    // 是否正在等待
        private string[] words;                // 内容

        public bool Finish = false;     // 是否结束的表示变量，仅作表达，不参与实际控制

        // 参数详解：
        // speed: 打字时间间隔
        // str: 保存需要打的文字列表
        // obj: text object

        // 调用方法(记住在Start里！！！):
        // void Start(){
        //     此脚本附加的object.GetComponent<daziji>().startPrint(speed, str, t);
        // }
        public void startPrint(float speed, string[] str, TextMeshProUGUI t){
            txt = t;
            resetTyper();

            length = str.Length;
            length--;
            spd = speed;
            isActive = true;
            isWaiting = false;
            words = new string[length];
            words = str;
            txt.text = "";
            Finish = false;
        }

        public void resetTyper(){
            nowLength = 0;
            length = 0;
            timer = 0;
            currentPos = 0;
            isWaiting = false;
            words = new string[0];
            txt.text = "";
            isActive = false;
        }

        void Update(){
            // 开始打字
            if (!isWaiting){
                if (isActive){
                    // 计时器变化
                    timer += Time.deltaTime;
                    if (timer >= spd){
                        timer = 0;
                        words[nowLength] = words[nowLength].Replace("\\n", "\n");
                        currentPos++;
                        txt.text = words[nowLength].Substring (0,currentPos);
                        if(currentPos>=words[nowLength].Length) {
                            timer = 0;
                            isWaiting = true;
                            currentPos = 0;
                            txt.text = words[nowLength];
                            nowLength++; 
                        }
                    }
                }
            }
        }

        public void TyperContinue()
        {
            if (!isWaiting) return;
            if (nowLength > length || !isActive){
                Finish = true;
                isActive = false;
                // txt.text = "";
            }else{
                isWaiting = false;
                timer = 0;
            }
        }

        public void TyperSkip()
        {
            if (isWaiting) return;
            timer = 0;
            currentPos = 0;
            txt.text = words[nowLength];
            if (nowLength >= length){
                nowLength = 0;
                isActive = false;
            }else{
                nowLength++;
            }
        }
    }
}
