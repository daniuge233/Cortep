using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 播放器
    // 这只是为了便于Unity API调用做的中转函数
    // 一个播放器应该用AudioHandler.SummonSoundPlayer().init(clip).Play();
    // 禁止直接new SoundPlayer()!!!否则该播放器将不参与生命周期更新。
    // 除非你是齐天大圣不小心改错生死簿了（
    public class SoundPlayer
    {
        public AudioSource player;

        public bool destroyAfterPlaying;
        public bool played;
        public bool pause;

        public SoundPlayer()
        {
            player = new GameObject("AudioPlayer").AddComponent<AudioSource>();
        }

        public void init(AudioClip clip, bool destroyAfterPlaying = true, float volume = 1.0f, bool loop = false)
        {
            player.clip = clip;
            player.volume = volume;
            player.loop = loop;
            
            this.destroyAfterPlaying = destroyAfterPlaying;
        }

        public void Play()
        {
            played = true;
            pause = false;
            player.Play();
        }

        public void Pause()
        {
            pause = true;
            player.Pause();
        }
    }
}
