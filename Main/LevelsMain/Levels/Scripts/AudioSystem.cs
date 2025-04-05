using UnityEngine;

namespace LevelsMain
{
    // 音  游  核  心(音乐播放器)
    public class AudioSystem : MonoBehaviour
    {
        public AudioSource AudioSource;

        public bool isPlaying = false;

        // Start is called before the first frame update
        void Awake()
        {
            gameObject.AddComponent<AudioSource>();
            AudioSource = gameObject.GetComponent<AudioSource>();
        }

        public void Play(AudioClip clip)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
            isPlaying = true;
        }
    }
}