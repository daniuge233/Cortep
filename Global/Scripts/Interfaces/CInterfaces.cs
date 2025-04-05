using System.Collections;
using UnityEngine;

using LevelsMain;

public interface ICUI
{
    Vector3 Smooth(Vector3 current, Vector3 target, float smoothDampSpd, ref float elapsedTime);
    IEnumerator Entering(GameObject obj, Vector3 tar, float time);
    IEnumerator Outting(GameObject obj, Vector3 tar, float time);
    IEnumerator ShowToast(string message, float showTime);

    IEnumerator ShowClickAnimation(Vector3 center, Sprite spr);
}

public interface ChartsAPI
{
    ChartAPIs GetChartController();
}