using System.IO;
using UnityEngine;

namespace LevelsMain
{
    public class MapReader : MonoBehaviour
    {

        Map ReadMap(int MapID)
        {
            string jsonData = ReadData(MapID);
            if (jsonData == "Null")
            {
                return null;
            }
            Map d = JsonUtility.FromJson<Map>(jsonData);

            return d;
        }

        //��ȡ�ļ�
        public string ReadData(int MapID)
        {
            //string���͵����ݳ���
            string readData;
            //��ȡ��·��
            string fileUrl = Application.streamingAssetsPath + "\\" + MapID + ".json";
            //��ȡ�ļ�
            using (StreamReader sr = File.OpenText(fileUrl))
            {
                //���ݱ���
                readData = sr.ReadToEnd();
                sr.Close();
            }

            if (string.IsNullOrEmpty(readData))
            {
                return "Null";
            }
            //��������
            return readData;
        }
    }
}