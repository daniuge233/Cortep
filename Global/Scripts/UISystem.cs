using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// UI系统，负责控制动画和UI
// 以及所有非线性插值
public class UISystem : MonoBehaviour, ICUI
{
    public Font font;

    // 平滑插值
    // 这里使用了Sin函数的平滑曲线
    // 实现了Sin函数的先加速再减速的效果。
    // smoothDampSpd为0.1时，平滑插值的时间约为1秒(1.042939sec)。
    // smoothDampSpd越小，则平滑速度越慢。
    // 这里绝对不要改成协程函数！！！！！
    Vector3 ICUI.Smooth(Vector3 current, Vector3 target, float smoothDampSpd, ref float elapsedTime)
    {
        elapsedTime += smoothDampSpd * Time.deltaTime;

        // 这一行需要单独解释：
        // x = Sin(x - Pi / 2) / 2 + 0.5可以使Sin的0 ~ 1插值发生在(0, 0)到(Pi, 1)之内
        // x * Pi可以使插值发生在(0, 0)和(1, 1)之内
        // Clamp01可以保证插值范围不会超过0 ~ 1
        // 不服去函数计算器里算一下
        // 这段代码转换为函数是: y = Sin((x-0.5) * Pi) / 2 + 0.5 {1>=x>=0}
        float x = Mathf.Clamp01(Mathf.Sin((elapsedTime - 0.5f) * Mathf.PI) * 0.5f + 0.5f);

        Vector3 num2 = Vector3.Lerp(current, target, x);
        return num2;
    }


    // 非线性上浮淡入
    IEnumerator ICUI.Entering(GameObject obj, Vector3 tar, float time)
    {
        while (!(Mathf.Abs(obj.transform.position.x - tar.x) > 0.001f)
                && !(Mathf.Abs(obj.transform.position.y - tar.y) > 0.001f)
                && !(Mathf.Abs(obj.transform.position.z - tar.z) > 0.001f))
        {
            Vector3 currentVelocity = Vector3.zero;
            obj.transform.position = Vector3.SmoothDamp(obj.transform.position, tar, ref currentVelocity, time);

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + time * Time.deltaTime * 3.75f);
            yield return null;
        }

        yield return null;
    }

    // 非线性下浮淡出
    // 原理同上
    IEnumerator ICUI.Outting(GameObject obj, Vector3 tar, float time)
    {
        while (!(Mathf.Abs(obj.transform.position.x - tar.x) > 0.001f)
                && !(Mathf.Abs(obj.transform.position.y - tar.y) > 0.001f)
                && !(Mathf.Abs(obj.transform.position.z - tar.z) > 0.001f))
        {
            Vector3 currentVelocity = Vector3.zero;
            obj.transform.position = Vector3.SmoothDamp(obj.transform.position, tar, ref currentVelocity, time);

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - time * Time.deltaTime * 3.75f);

            yield return null;
        }

        yield return null;
    }

    // 这是一个协程函数
    // 用于显示Toast
    // 调用需要这样：StartCoroutine(ICUI.ShowToast());
    IEnumerator ICUI.ShowToast(string message, float showTime)
    {
        // 初始化背景板
        GameObject obj_bg = new("Toast_bg", typeof(Image), typeof(ContentSizeFitter), typeof(HorizontalLayoutGroup));
        Image img = obj_bg.GetComponent<Image>();
        ContentSizeFitter csf = obj_bg.GetComponent<ContentSizeFitter>();
        HorizontalLayoutGroup hlg = obj_bg.GetComponent<HorizontalLayoutGroup>();
        img.color = Color.white;
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        hlg.childControlWidth = hlg.childControlHeight = hlg.childForceExpandHeight = hlg.childForceExpandWidth = true;
        obj_bg.transform.SetParent(GameObject.Find("Canvas").transform);
        RectTransform obj_bgRectTransform = obj_bg.GetComponent<RectTransform>(); // 获取RectTransform组件

        // 初始化文字
        GameObject obj_txt = new("Toast_txt", typeof(Text));
        Text t = obj_txt.GetComponent<Text>();
        t.text = message;
        t.color = Color.black;
        t.fontSize = 25;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = font;
        obj_txt.transform.SetParent(obj_bg.transform);

        // 获取屏幕的宽度和高度
        float screenHeight = Screen.height;

        // 计算超出屏幕的Y坐标
        float offsetY = screenHeight * 0.5f + obj_bgRectTransform.sizeDelta.y * 0.5f;
        float targetOffsetY = screenHeight * 0.5f + obj_bgRectTransform.sizeDelta.y * 0.5f - 90;

        // 设置背景板的初始位置超出屏幕范围
        obj_bgRectTransform.anchoredPosition = new Vector2(0, offsetY);

        // 平滑移动背景板到目标位置
        float elapsedTime = 0;
        while (Mathf.Abs(obj_bgRectTransform.anchoredPosition.y - targetOffsetY) > 0.01f)
        {
            obj_bgRectTransform.anchoredPosition = ((ICUI)this).Smooth(obj_bgRectTransform.anchoredPosition, new Vector2(0, targetOffsetY), 0.2f, ref elapsedTime);

            yield return null;
        }

        // 等待
        yield return new WaitForSeconds(showTime);

        // 平滑移动背景板回到屏幕外的原位置
        elapsedTime = 0;
        while (Mathf.Abs(obj_bgRectTransform.anchoredPosition.y - offsetY) > 0.01f)
        {
            obj_bgRectTransform.anchoredPosition = ((ICUI)this).Smooth(obj_bgRectTransform.anchoredPosition, new Vector2(0, offsetY), 0.2f, ref elapsedTime);

            yield return null;
        }

        // 销毁对象
        Destroy(obj_bg);
    }

    // Note点击特效
    // 懒得学Untiy Animation了，直接写出来算
    // 反正效果一样 
    // 大不了以后慢慢学(
    // 只不过这段代码好答辩山(((((
    IEnumerator ICUI.ShowClickAnimation(Vector3 center, Sprite spr)
    {

        // 初始化
        // 外圆
        // 创建Object
        GameObject l_t, r_t, l_b, r_b;
        float centerX = center.x, centerY = center.y;
        l_t = AnimClick_CreateCircle(new Vector3(centerX - 0.125f, centerY + 0.125f, 0), 0, spr);
        r_t = AnimClick_CreateCircle(new Vector3(centerX + 0.125f, centerY + 0.125f, 0), -90, spr);
        l_b = AnimClick_CreateCircle(new Vector3(centerX - 0.125f, centerY - 0.125f, 0), 90, spr);
        r_b = AnimClick_CreateCircle(new Vector3(centerX + 0.125f, centerY - 0.125f, 0), 180, spr);

        // SpriteRenderer
        SpriteRenderer l_t_sr = l_t.GetComponent<SpriteRenderer>(),
                                r_t_sr = r_t.GetComponent<SpriteRenderer>(),
                                l_b_sr = l_b.GetComponent<SpriteRenderer>(),
                                r_b_sr = r_b.GetComponent<SpriteRenderer>();

        // 目标位置
        Vector3 l_t_tar = new Vector3(centerX - 1, centerY + 1, 0),
                    r_t_tar = new Vector3(centerX + 1, centerY + 1, 0),
                    l_b_tar = new Vector3(centerX - 1, centerY - 1, 0),
                    r_b_tar = new Vector3(centerX + 1, centerY - 1, 0);
        // 时间
        float time = 0.05f;
        // 速度
        Vector3 cv_l_t, cv_r_t, cv_l_b, cv_r_b;
        cv_l_t = cv_r_t = cv_l_b = cv_r_b = Vector3.zero;

        // 内圆
        // 创建Object
        GameObject l_t_i, r_t_i, l_b_i, r_b_i;
        l_t_i = AnimClick_CreateCircle(new Vector3(centerX - 0.125f, centerY + 0.125f, 0), 0, spr);
        r_t_i = AnimClick_CreateCircle(new Vector3(centerX + 0.125f, centerY + 0.125f, 0), -90, spr);
        l_b_i = AnimClick_CreateCircle(new Vector3(centerX - 0.125f, centerY - 0.125f, 0), 90, spr);
        r_b_i = AnimClick_CreateCircle(new Vector3(centerX + 0.125f, centerY - 0.125f, 0), 180, spr);

        // SpriteRenderer
        SpriteRenderer l_t_sr_i = l_t_i.GetComponent<SpriteRenderer>(),
                                r_t_sr_i = r_t_i.GetComponent<SpriteRenderer>(),
                                l_b_sr_i = l_b_i.GetComponent<SpriteRenderer>(),
                                r_b_sr_i = r_b_i.GetComponent<SpriteRenderer>();

        // 目标位置
        Vector3 l_t_tar_i = new Vector3(centerX - 0.5f, centerY + 0.5f, 0),
                    r_t_tar_i = new Vector3(centerX + 0.5f, centerY + 0.5f, 0),
                    l_b_tar_i = new Vector3(centerX - 0.5f, centerY - 0.5f, 0),
                    r_b_tar_i = new Vector3(centerX + 0.5f, centerY - 0.5f, 0);
        // 速度
        Vector3 cv_l_t_i, cv_r_t_i, cv_l_b_i, cv_r_b_i;
        cv_l_t_i = cv_r_t_i = cv_l_b_i = cv_r_b_i = Vector3.zero;

        while (true)
        {
            // 外圆
            l_t.transform.position = Vector3.SmoothDamp(l_t.transform.position, l_t_tar, ref cv_l_t, time);
            r_t.transform.position = Vector3.SmoothDamp(r_t.transform.position, r_t_tar, ref cv_r_t, time);
            l_b.transform.position = Vector3.SmoothDamp(l_b.transform.position, l_b_tar, ref cv_l_b, time);
            r_b.transform.position = Vector3.SmoothDamp(r_b.transform.position, r_b_tar, ref cv_r_b, time);

            // 内圆
            l_t_i.transform.position = Vector3.SmoothDamp(l_t_i.transform.position, l_t_tar_i, ref cv_l_t_i, time);
            r_t_i.transform.position = Vector3.SmoothDamp(r_t_i.transform.position, r_t_tar_i, ref cv_r_t_i, time);
            l_b_i.transform.position = Vector3.SmoothDamp(l_b_i.transform.position, l_b_tar_i, ref cv_l_b_i, time);
            r_b_i.transform.position = Vector3.SmoothDamp(r_b_i.transform.position, r_b_tar_i, ref cv_r_b_i, time);

            // 统一以左上角外圆环为循环终点
            if (Mathf.Abs(l_t.transform.position.x - l_t_tar.x) <= 0.001f && Mathf.Abs(l_t.transform.position.y - l_t_tar.y) <= 0.001f)
            {
                break;
            }

            yield return null;
        }

        while (l_t_sr.color.a > 0)
        {

            l_t_sr.color
            = r_t_sr.color
            = l_b_sr.color
            = r_b_sr.color
            = l_t_sr_i.color
            = r_t_sr_i.color
            = l_b_sr_i.color
            = r_b_sr_i.color
            = new Color(255, 255, 255, l_t_sr.color.a - Time.deltaTime * 5);

            yield return null;
        }

        Destroy(l_t); Destroy(r_t); Destroy(l_b); Destroy(r_b); Destroy(l_t_i); Destroy(r_t_i); Destroy(l_b_i); Destroy(r_b_i);
    }

    GameObject AnimClick_CreateCircle(Vector3 startPosition, float rotation, Sprite spr)
    {
        GameObject obj = new GameObject("AnimClick", typeof(SpriteRenderer));
        obj.transform.eulerAngles = new Vector3(0, 0, rotation);
        obj.transform.position = startPosition;
        obj.GetComponent<SpriteRenderer>().sprite = spr;

        return obj;
    }
}
