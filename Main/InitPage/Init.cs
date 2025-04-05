using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using DeserializaedData;

// 根据dataPath/Global/Data/init.json中存储的数据
// 执行游戏运行前初始化操作
// 初始化包括游戏基本运行参数、关卡信息等。
// 同时也是全局数据库脚本，使用DDOL.cs实现场景间传递
// 这里不是存档的加载脚本，存档不要用JSON，要用BinaryFormatter
public class Init : MonoBehaviour
{
    public Data InitData;

    public Text tmp;

    private void InitLoad()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("init");
        if (jsonFile != null)
        {
            string data = jsonFile.text;

            // 我在Data里加了一个Chapter的List，又在Chapter的List里加了Level的List，又在Level的List里加了int的List
            // 但它依然能读取！！！
            // すこい！！
            InitData = JsonUtility.FromJson<Data>(data);   

            SceneManager.LoadScene("HomePage");
        }
        else
        {
            Debug.LogError("初始化数据文件不存在");
            tmp.text = "错误：初始化数据文件不存在";
        }
    }

    private void Start()
    {
        InitLoad();
    }
}