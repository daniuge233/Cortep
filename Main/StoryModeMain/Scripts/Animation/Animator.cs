using UnityEngine;
using UnityEditor;

// 这个脚本用于播放动画，此脚本会将设置的每一帧依次附加到role上.
// 支持定义多个动画
// 最大支持一个动画200000帧，需要更多或觉得太多请更改第60行的数组长度
namespace StoryModeMain
{
    public struct anim{
        public float frameCount;
        public Sprite[] frames;
        public float Interval;
    };

    public class Animator : MonoBehaviour {
        
        public GameObject role;     // 角色
        // public float interval = 0;      // 时间间隔
        public int defaultAnimationPosition = 0;        // 默认动画播放帧(reset时使用)

        [Space]
        [Header("动画设置\n移动动画每个的帧数必须一致,不然会有空帧")]
        public int[] framesForEachAnimations = new int[0];          // 每个动画的帧数
        public Sprite[] texturesForEachAnimations = new Sprite[0];        // 每个动画的每一帧(技术原因，请手动设置长度，抱歉...)，原理见用户手册
        public float[] intervals = new float[0];          // 每个动画的时间间隔

        public anim[] animations;     // 保存每个动画信息

        [HideInInspector]
        public bool canPlay = false;     // 是否允许播放
        [HideInInspector]
        public int currentAnimationId = 0;       // 当前动画ID
        private int currentPlayPosition;      // 当前播放位置(帧)
        private float timer = 0;          // 计时器
        private float targetTime = 0;     // 需要等待至的时间
        private SpriteRenderer sr;        // SpriteRenderer

        // 播放函数(在需要开始播放是调用该函数并指定要播放动画的id)
        public void playAnimation(int id){
            canPlay = true;
            currentAnimationId = --id;
        }

        // 重置函数(在需要停止播放时调用该函数)
        public void resetAnimation(){
            canPlay = false;
            sr.sprite = animations[currentAnimationId].frames[defaultAnimationPosition];
            currentAnimationId = 0;
        }

        void Awake(){
            // 初始化
            // 添加SpriteRenderer
            role.AddComponent<SpriteRenderer>();
            // 初始化参数变量
            // s为当前数据帧，p为当前帧，i为当前动画
            int s = 0, n = 0, p = 0;
            // 定义动画结构体
            animations = new anim[framesForEachAnimations.Length];
            // 获取SpriteRenderer插件
            sr = role.GetComponent<SpriteRenderer>();
            // 循环赋值结构体参数
            for (int i = 0; i < framesForEachAnimations.Length; i++){
                animations[i].frameCount = framesForEachAnimations[i];      // 向结构体传输每个动画的帧数
                animations[i].frames = new Sprite[200000];
                for (; s < n + framesForEachAnimations[i]; s++){      // 循环传输每一帧
                    animations[i].frames[p] = texturesForEachAnimations[s];     // 传输一帧
                    p++;
                }
                n += framesForEachAnimations[i];
                p = 0;

                // 传输时间间隔
                animations[i].Interval = intervals[i];
            }
        }


        void Update(){
            timer += Time.deltaTime;
            if (canPlay){
                if (timer >= targetTime){      // 判断延迟时间到达
                    targetTime = timer + animations[currentAnimationId].Interval;      // 获取延迟目标时间
                    sr.sprite = animations[currentAnimationId].frames[currentPlayPosition];     // 处理动画播放
                    if (currentPlayPosition >= animations[currentAnimationId].frameCount-1){   // 处理重播
                        currentPlayPosition = 0;
                    }else{      // 处理下一帧
                        currentPlayPosition++;
                    }
                } 
            } else {
                targetTime = 0;         // 反鬼畜
            }
        }
    }
}
