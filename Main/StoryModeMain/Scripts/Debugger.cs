using UnityEngine;

namespace StoryModeMain
{
    public class Debugger : MonoBehaviour
    {
        public UserEventHandler UEH;
        public GameObject InteractiveNPC;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UEH.NPCEvent(InteractiveNPC);
            }
        }
    }
}