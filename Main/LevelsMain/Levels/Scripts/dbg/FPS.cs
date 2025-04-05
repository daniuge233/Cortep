using UnityEngine;
using UnityEngine.UI;

namespace LevelsMain
{
    public class FPS : MonoBehaviour
    {
        public Text fpsText;

        void Update()
        {
            var fps = 1.0f / Time.deltaTime;
            // fpsText.text = $"FPS: {fps}";
        }
    }
}