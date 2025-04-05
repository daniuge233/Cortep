using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform ThisTrasform = null;
    //总共抖动时间
    public float ShakeTime = 2.0f;
    //在任何方向上偏移的距离
    public float ShakeAmount = 4.0f;
    //相机移动到震动点的速度
    public float ShakeSpeed = 3.0f;

    public void StartShake()
    {
        ThisTrasform = GetComponent<Transform>();
        //开始震动
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        //存下当前相机位置
        Vector3 OrigPosition = ThisTrasform.localPosition;
        //记下运行时间
        float ElapsedTime = 0.0f;
        //重复此步骤以获得总震动时间
        while (ElapsedTime < ShakeTime)
        {
            //在单位球上随机取点
            Vector3 RandomPoint = OrigPosition + Random.insideUnitSphere * ShakeAmount;
            //更新相机位置
            ThisTrasform.localPosition = Vector3.Lerp(ThisTrasform.localPosition, RandomPoint, Time.deltaTime * ShakeSpeed);
            yield return null;
            //更新时间
            ElapsedTime += Time.deltaTime;
        }
        //恢复相机位置
        ThisTrasform.localPosition = OrigPosition;
    }

    private void Start()
    {
        StartShake();
    }
}