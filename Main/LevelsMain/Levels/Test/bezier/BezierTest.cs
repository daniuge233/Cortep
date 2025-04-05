using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTest : MonoBehaviour
{
    public List<Transform> wayPoint = new List<Transform>();   //路点信息（首尾表示起点和终点，中间为相对n阶偏移点）
    public int pointCount = 10;     //曲线上点的个数
    private Vector3[] linePointList;

    private void Start()
    {
        //Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Move());
        }
    }

    public void Init()
    {
        List<Vector3> newP = new List<Vector3>();
        for (int i = 0; i < wayPoint.Count; i++)
        {
            if (!newP.Contains(wayPoint[i].position))
            {
                newP.Add(wayPoint[i].position);
            }
        }
        linePointList = BezierUtils.GetBeizerPointList(pointCount, newP);
    }

    /// <summary>
    /// 在Scene视图划线
    /// </summary>
    public void OnDrawGizmos()
    {
        Init();
        Gizmos.color = Color.yellow;
        for (int i = 0; i < linePointList.Length - 1; i++)
        {
            Debug.DrawLine(linePointList[i], linePointList[i + 1], Color.yellow);
        }
    }

    public IEnumerator Move()
    {
        for (int i = 0; i < linePointList.Length - 1; i++)
        {
            transform.position = Vector3.Lerp(linePointList[i], linePointList[i + 1], 1);
            yield return new WaitForSeconds(0.02f);
        }
    }


}
