using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelsMain_O.API
{
    public static class index
    {
        public static void InitUI() { MainSystem.InitUI(); }

        public static Ring AddRing(int id)
        {
            Ring ring = new Ring(id);
            return ring;
        }

        public static Note AddNote(int RingID, Note.NoteType type, int[] args)
        {
            switch(type)
            {
                case Note.NoteType.Duang:
                    return new Duang(RingID, args[0], args[1]);

                case Note.NoteType.Full:
                    return new Full(RingID, args[0], args[1], args[2]);

                case Note.NoteType.Licky:
                    return new Licky(RingID, args[0], args[1]);
                    
                case Note.NoteType.Mining:
                    return new Mining(RingID, args[0], args[1], args[2]);
            }

            return null;
        }

        public static void StartGame() { MainSystem.StartGame(); }
    }
}