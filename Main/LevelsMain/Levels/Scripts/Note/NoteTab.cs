using UnityEngine;

namespace LevelsMain
{
    // 这是用来标记note信息的
    // 作为传递note数据的中间过渡区
    // 所有来自Note.cs的专属数据均通过这个脚本传递至Arbiter.cs或其他需要的位置
    public class NoteTab : MonoBehaviour
    {
        public int type;       // 类型
        public bool isHold = false;
        public bool isHolded = false;
        public float goodAngle = 0, goodAngle_ = 0, perfectAngle = 0, perfectAngle_ = 0;
        public Note n;
    }
}