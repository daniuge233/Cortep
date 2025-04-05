using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    // 音  游  核  心（音频播放器）
    public class AudioHandler : MonoBehaviour
    {
        // 仅用作更新，不被其他类调用
        private static List<SoundPlayer> SoundPlayers = new();

        // 创建一个新的播放器
        public static SoundPlayer SummonSoundPlayer()
        {
            SoundPlayer sp = new();
            SoundPlayers.Add(sp);

            return sp;
        }

        // 用于更新播放器的函数
        // 主要是处理播放器的生命周期
        private void UpdatePlayers()
        {
            foreach(SoundPlayer player in SoundPlayers)
            {
                if (!player.destroyAfterPlaying) continue;

                // 播放器生命周期结束
                if (player.played && !player.pause && !player.player.isPlaying)
                {
                    SoundPlayers.Remove(player);
                    Destroy(player.player);
                }
            }
        }

        private void Update()
        {
            UpdatePlayers();
        }
    }
}