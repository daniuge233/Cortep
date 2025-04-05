using UnityEngine;

// 这个脚本用于播放动画，此脚本会将设置的每一帧依次附加到role上.
// 支持定义多个动画
// 最大支持一个动画200000帧，需要更多或觉得太多请更改第60行的数组长度
public struct anim
{
    public float frameCount;
    public Sprite[] frames;
    public float Interval;
};

public class Animation : MonoBehaviour
{

    public GameObject role;     // 角色
    // public float interval = 0;      // 时间间隔
    public int defaultAnimationPosition = 0;        // 默认动画播放帧(reset时使用)
    public int[] framesForEachAnimations = new int[0];          // 每个动画的帧数
    public Sprite[] texturesForEachAnimations = new Sprite[0];        // 每个动画的每一帧技术原因，请手动设置长度，抱歉...)，原理见用户手册
    public float[] intervals = new float[0];          // 每个动画的时间间隔

    public anim[] animations;     // 保存每个动画信息

    public static bool canPlay = false;     // 是否允许播放
    public static int currentAnimationId = 0;       // 当前动画ID
    public static int currentPlayPosition;      // 当前播放位置(帧)
    public static float timer = 0;          // 计时器
    public static float targetTime = 0;     // 需要等待至的时间
    public static SpriteRenderer sr;        // SpriteRenderer

    public bool isLoop = false;

    // 播放函数(在需要开始播放是调用该函数并指定要播放动画的id)
    public void playAnimation(int id, bool isLoop = false)
    {
        canPlay = true;
        this.isLoop = isLoop;
        currentAnimationId = --id;
    }

    // 重置函数(在需要停止播放时调用该函数)
    public void resetAnimation()
    {
        canPlay = false;
        sr.sprite = animations[currentAnimationId].frames[defaultAnimationPosition];
        currentAnimationId = 0;
    }

    void Awake()
    {
        // 初始化
        // 添加SpriteRenderer
        role.AddComponent<SpriteRenderer>();
        // 初始化参数变量
        int s = 0, n = 0, p = 0;
        // 定义动画结构体
        animations = new anim[framesForEachAnimations.Length];
        // 获取SpriteRenderer插件
        sr = role.GetComponent<SpriteRenderer>();
        // 循环赋值结构体参数
        for (int i = 0; i < framesForEachAnimations.Length; i++)
        {
            animations[i].frameCount = framesForEachAnimations[i];      // 向结构体传输每个动画的帧数
            animations[i].frames = new Sprite[200000];
            for (; s < n + framesForEachAnimations[i]; s++)
            {      // 循环传输每一帧
                animations[i].frames[p] = texturesForEachAnimations[s];     // 传输一帧
                p++;
            }
            n += framesForEachAnimations[i];
            p = 0;

            // 传输时间间隔
            animations[i].Interval = intervals[i];
        }
    }

    private void UpdateAnim()
    {
        targetTime = timer + animations[currentAnimationId].Interval;      // 获取延迟目标时间
        sr.sprite = animations[currentAnimationId].frames[currentPlayPosition];     // 处理动画播放
        if (currentPlayPosition >= animations[currentAnimationId].frameCount - 1)
        {   // 处理重播   
            if (isLoop)
            {
                currentPlayPosition = 0;
            }
            else
            {
                resetAnimation();
            }
        }
        else
        {      // 处理下一帧
            currentPlayPosition++;
        }
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (canPlay)
        {
            if (timer >= targetTime)
            {      // 判断延迟时间到达
                UpdateAnim();
            }
        }
        else
        {
            targetTime = 0;         // 反鬼畜
        }
    }
}