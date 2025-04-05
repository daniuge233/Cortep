using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform ThisTrasform = null;
    //�ܹ�����ʱ��
    public float ShakeTime = 2.0f;
    //���κη�����ƫ�Ƶľ���
    public float ShakeAmount = 4.0f;
    //����ƶ����𶯵���ٶ�
    public float ShakeSpeed = 3.0f;

    public void StartShake()
    {
        ThisTrasform = GetComponent<Transform>();
        //��ʼ��
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        //���µ�ǰ���λ��
        Vector3 OrigPosition = ThisTrasform.localPosition;
        //��������ʱ��
        float ElapsedTime = 0.0f;
        //�ظ��˲����Ի������ʱ��
        while (ElapsedTime < ShakeTime)
        {
            //�ڵ�λ�������ȡ��
            Vector3 RandomPoint = OrigPosition + Random.insideUnitSphere * ShakeAmount;
            //�������λ��
            ThisTrasform.localPosition = Vector3.Lerp(ThisTrasform.localPosition, RandomPoint, Time.deltaTime * ShakeSpeed);
            yield return null;
            //����ʱ��
            ElapsedTime += Time.deltaTime;
        }
        //�ָ����λ��
        ThisTrasform.localPosition = OrigPosition;
    }

    private void Start()
    {
        StartShake();
    }
}