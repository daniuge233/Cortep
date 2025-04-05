using LevelsMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 啊啊啊啊最难的判定器啊啊啊啊
// 同时也是bug聚集地
public class Arbiter : MonoBehaviour
{
    public Sprite Square;
    public Text dbgtxt;

    private AssessLine assessLine;
    public MainSystem ms;
    public BPMTimer bpmtimer;

    // 普通判定
    // 返回值:
    // 0: bad/miss
    // 1: good
    // 2: perfect
    private int Arbit(NoteTab nt)
    {
        float line = assessLine.GetLine().transform.eulerAngles.z;
        float line_ = assessLine.GetLine().transform.eulerAngles.z - 180;
        float linep = assessLine.GetLine().transform.eulerAngles.z + 180;

        // perfect判定
        if ((line >= nt.perfectAngle_ && line <= nt.perfectAngle) ||
             (line_ >= nt.perfectAngle_ && line_ <= nt.perfectAngle) ||
             (linep >= nt.perfectAngle_ && linep <= nt.perfectAngle))
        {
            return 2;
        }

        // good判定
        else if ((line >= nt.goodAngle_ && line <= nt.goodAngle) ||
                    (line_ >= nt.goodAngle_ && line_ <= nt.goodAngle) ||
                    (linep >= nt.goodAngle_ && linep <= nt.goodAngle))
        {
            return 1;
        }

        // bad判定
        else if (line < nt.goodAngle_ ||
                    line_ < nt.goodAngle_ ||
                    linep < nt.goodAngle_)
        {
            return 0;
        }

        // miss判定
        return 0;
    }

    // 初始化
    private void Init()
    {
        assessLine = gameObject.GetComponent<AssessLine>();
        ms = gameObject.GetComponent<MainSystem>();
        Input.multiTouchEnabled = true;
    }

    // 根据note对象获取Note
    private Note GetNote(GameObject noteObj)
    {
        Note n = null;
        for (int i = 0; i < ms.notes.Count; i++)
        {
            if (ms.notes[i].GetObj() == noteObj)
            {
                n = ms.notes[i];
            }
        }
        return n;
    }

    // 删除某Note
    private void Destroy(GameObject note)
    {
        Note n = GetNote(note);
        Destroy((UnityEngine.Object)n.GetObj());
        ms.notes.Remove(n);
    }

    // 初始化
    void Start()
    {
        Init();
    }

    // 判断手指状态
    private void Swi(Touch touch, GameObject targetNote, NoteTab nt)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:      // 手指单点
                Click(nt, targetNote);
                break;
            case TouchPhase.Moved:     // 手指移动
                MoveAndHold(nt, touch);
                break;
            case TouchPhase.Stationary:     // 手指按住
                MoveAndHold(nt, touch);
                break;
            case TouchPhase.Ended:      // 手指离开
                nt.isHold = false;
                break;
        }
    }

    // 处理单点
    private void Click(NoteTab nt, GameObject targetNote)
    {
        int targetType = nt.type;
        if (targetType == 0)
        {
            int result = Arbit(nt);
            // 这里是判定之后的事, 比如根据结果放动画巴拉巴拉的
            // 删除原note
            this.Destroy(targetNote);
        }
        else if (targetType == 2)
        {

            Destroy(targetNote);
        }
    }

    // 处理移动或按住
    private void MoveAndHold(NoteTab nt, Touch touch)
    {
        // hold解决方案想出来之前这里没什么卵用
        // (以及mining)
        if (nt.type == 1 || nt.type == 3)
        {
            nt.isHolded = true;
            if (!nt.isHold)
            {
                nt.isHold = true;
            }
        }
    }

    // Mining专属判定之调度部分
    private void MiningArbet(NoteTab nt, Note n)
    {
        if (nt.isHold == true)
        {
            Mining(n);
        }
        else if (nt.isHolded == true && nt.isHold == false)
        {
            Destroy((UnityEngine.Object)n.GetObj());
            Destroy((UnityEngine.Object)n.GetMiningObjectInside());
            ms.notes.Remove(n);
            // 放bad/miss动画
        }
    }

    // mining专属判定按住部分
    private void Mining(Note n)
    {
        float increment = n.holdTime * Time.deltaTime * 0.04f;
        float curX = n.GetMiningObjectInside().transform.localScale.x, curY = n.GetMiningObjectInside().transform.localScale.y;
        n.GetMiningObjectInside().transform.localScale = new Vector3(curX + increment, curY + increment, 0);
        if (n.GetMiningObjectInside().transform.localScale.x >= 0.75f && n.GetMiningObjectInside().transform.localScale.y >= 0.75f)
        {
            Destroy((UnityEngine.Object)n.GetObj());
            Destroy((UnityEngine.Object)n.GetMiningObjectInside());
            ms.notes.Remove(n);
            // 放perfect动画
        }
    }

    // 这里通过协程函数控制StartNote沿轨道移动
    // 判断是否到达的方法是计算角度, 千万不要用碰撞器或坐标！！！！！
    // 还没测试过。
    public IEnumerator MoveStartNote(Note note)
    {
        GameObject start = note.GetObjsForFull()[0];
        GameObject end = note.GetObjsForFull()[1];

        float angleStart = start.transform.eulerAngles.z + 90;
        float angleEnd = end.transform.eulerAngles.z;
        float angle_ = angleEnd - angleStart;
        bool flag = false; // 标记正反向 false为正 true为负
        if (note.full.length < 0) flag = true;
        angle_ = Mathf.Abs(angle_);
        float angleP = angle_ / note.ActiveKeep; // 每拍的增量

        const float r = 3.1225f;

        float cBPM = 0;

        while (Mathf.Abs(start.transform.eulerAngles.z - end.transform.eulerAngles.z) > 0.01f)
        {
            cBPM += bpmtimer.deltaBPM();
            if (cBPM >= 1)
            {
                // 更新角度
                if (!flag)
                    angleStart -= angleP;
                else
                    angleStart += angleP;

                // 限制角度在0到360度之间
                // angleStart = Mathf.Repeat(angleStart, 360f);

                start.transform.eulerAngles = new Vector3(0, 0, angleStart - 90);

                // 更新位置
                float x = r * Mathf.Cos(angleStart * Mathf.PI / 180);
                float y = r * Mathf.Sin(angleStart * Mathf.PI / 180);

                start.transform.position = new Vector3(x, y, start.transform.position.z);
            }

            yield return null;
        }

        // 最后确保设置正确的位置和角度
        start.transform.eulerAngles = new Vector3(0, 0, angleEnd);
        start.transform.position = end.transform.position;

        note.full_isFinish = true;

        yield return null;
    }

    // 计算圆上任意一点位置
    // 通过触摸判断手指的位置, 然后通过那个坐标带入公式计算圆上坐标, 计算note位置
    // x1 = x0 + r * cos(ao * pi / 180) 
    // y1 = y0 + r * sin(ao * pi / 180)
    // 由于圆坐标为(0, 0), 所以省略。
    // 圆点坐标：(x0, y0)     半径：r    角度：a0      圆周率：pi
    // 这段暂时没用了
    // private Vector3 FixPos(Vector3 touchPos)
    // {

    //     Vector3 targetDir = touchPos - Vector3.zero;
    //     float angle = 90 - Vector2.Angle(Vector2.up, targetDir);    
    //     const float pi = Mathf.PI;
    //     const float r = 3.1225f;

    //     double x = r * Math.Cos(angle * pi / 180);
    //     double y = r * Math.Sin(angle * pi / 180);

    //     if (touchPos.x < 0) x = -x;
    //     Vector3 pos = new((float)x, (float)y, -9.7f);

    //     return pos;
    // }

    // 触摸器
    RaycastHit2D TouchGet(Touch t)
    {
        Ray FRay = Camera.main.ScreenPointToRay(t.position);
        Vector3 touchPos = new Vector3(FRay.origin.x, FRay.origin.y, 20);
        // 从手指处垂直向内发射射线, 检测音符
        // 我真是个兲才
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector3.back, Mathf.Infinity, 1 << LayerMask.NameToLayer("notes"));

        return hit;
    }

    // Full专属判定
    // i指找到的Note在notes里的位置
    private void FullArbet(Note n, int i)
    {
        // 这里改为手指位置
        // 在StartNote移动过程中，判断手指是否一直处于碰撞器内
        foreach (Touch touch in Input.touches)
        {
            // 检测音符 单独写一个吧
            RaycastHit2D hit = TouchGet(touch);

            if (!hit) continue;
            GameObject tar = hit.collider.gameObject;
            if (tar == ms.notes[i].GetStartObj())
            {
                NoteTab nt = tar.GetComponent<NoteTab>();
                Note note = nt.n;
                if (tar == note.obj_start)
                {
                    if (note.full_isFinish == true)
                    {
                        List<GameObject> full_objects = note.GetObjsForFull();
                        for (int s = 0; s < full_objects.Count; s++)
                        {
                            Destroy(full_objects[i]);
                        }
                    }
                    else
                    {
                        // 根据判定信息播放动画
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                RaycastHit2D hit = TouchGet(touch);

                if (!hit) continue;
                GameObject targetNote = hit.collider.gameObject;
                NoteTab nt = targetNote.GetComponent<NoteTab>();
                if (nt != null && nt.type != 1 && nt.type != 3)
                {
                    Swi(touch, targetNote, nt);
                }
            }
        }

        for (int i = 0; i < ms.notes.Count; i++)
        {
            Note n = ms.notes[i];
            if (n.type == 3)
            {
                NoteTab nt = ms.notes[i].GetObj().GetComponent<NoteTab>();
                MiningArbet(nt, n);
                continue;
            }
            else if (n.type == 1)
            {
                FullArbet(n, i);
                continue;
            }
        }
    }
}
