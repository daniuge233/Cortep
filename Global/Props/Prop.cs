using UnityEngine;

// 对道具的定义
namespace Props
{
    public class Prop
    {
        private string Name;
        private string Introduction;

        public Prop(string name, string introduction)
        {
            Name = name;
            Introduction = introduction;
        }

        public string GetName() { return Name; }
        public string GetIntroduction() { return Introduction; }
    }
}