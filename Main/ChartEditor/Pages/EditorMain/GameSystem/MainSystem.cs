using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    public class MainSystem : MonoBehaviour
    {
        public static List<Ring> Rings = new();
        public static List<Behaviour> Components = new();
        public static int RingCnt = 0, NotesCnt = 0;

        private void OnEnable()
        {
            foreach(Component component in gameObject.GetComponents(typeof(Component)))
            {
                // 加一个try-catch是为了防止因非Behaviour脚本的对象无法转为Behaviour类型而导致的报错
                // 如Transform、Database等
                try
                { 
                    if (component == Camera.main || component.gameObject.layer != LayerMask.NameToLayer("Notes")) continue;
                    Components.Add((Behaviour)component);
                } 
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }
}