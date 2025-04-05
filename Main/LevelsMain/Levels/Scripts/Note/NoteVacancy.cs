using UnityEngine;

namespace LevelsMain
{
    // 音符空位, 用于定位每一个音符出现点
    // 共16个
    public class NoteVacancy
    {
        private int id;
        private GameObject obj;

        public NoteVacancy(Vector3 position, int id, MainSystem MainSystem)
        {
            obj = new GameObject("NoteVacancy");
            obj.transform.position = position;
            obj.transform.SetParent(MainSystem.mainParent.transform);
            this.id = id;
        }

        public GameObject GetObj()
        {
            return obj;
        }

        public int GetID()
        {
            return id;
        }
    }
}