using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    // 这个脚本保证GameObject在切换场景时不会销毁
    public class DDOL : MonoBehaviour
    {
        void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Database");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }
}