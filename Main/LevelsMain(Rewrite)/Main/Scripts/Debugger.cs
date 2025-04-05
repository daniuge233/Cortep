using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O
{
    public class Debugger : MonoBehaviour
    {
        private Note note;

        public GameObject noteObj;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                note = noteObj.GetComponent<NoteController>().Note;
                note.Click();
            }

            if (Input.GetKey(KeyCode.H))
            {
                note = noteObj.GetComponent<NoteController>().Note;
                note.Hold();
            }
        }

        public static void UpdateDebugText(string message)
        {
            DB.DebugText.text = message;
        }
    }
}