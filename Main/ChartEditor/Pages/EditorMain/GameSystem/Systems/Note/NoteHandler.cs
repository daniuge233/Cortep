using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartEditor.Main.GameSystem
{
    // 控制Notes的系统
    // 这里不负责判定，只负责Note的启动和删除
    public class NoteHandler : MonoBehaviour
    {
        // 在每一帧之后调度处理更新每个Note
        private void LateUpdate()
        {
            foreach (Ring ring in MainSystem.Rings)
            {
                UpdateNotesInRing(ring);
            }
        }
        // 调度处理每个Ring里的Notes
        private void UpdateNotesInRing(Ring ring)
        {
            foreach (Note note in ring.Notes)
            {
                UpdateNote(note);
            }
        }
        // 处理每个Note
        private void UpdateNote(Note note)
        {
            if (note.isDestroyed) return;

            ActiveNote(note);
            MissDetermine(note);
        }

        // 启动Note
        private void ActiveNote(Note note)
        {
            if (Timer.curBPM >= note.ActiveBeat && !note.isActive)
            {
                note.isActive = true;
                note.Active();
            }
        }

        // 判定Miss
        // 因为Miss的机制是任何Note都一样的
        // 所以可以在Handler里统一管理
        // Miss的判定机制是如果在第一次达成Perfect和Good「以后」出现了Bad即判定为Miss。
        // 是一种基于Bad的Miss判定方式（？
        private void MissDetermine(Note note) {}
    }
}