using System.Collections;
using UnityEngine;

namespace SettlementMain
{
    public class MainSystem : MonoBehaviour
    {
        public GameObject Rings_parent;
        public Camera Camera;

        private GameObject Camera_obj;

        private Vector3[] Positions =
        {
            new Vector3(10, 5, 0),
            new Vector3(-10, 5, 0),
            new Vector3(10, -5, 0),
            new Vector3(-10, -5, 0)
        };

        private IEnumerator Transform(int index)
        {
            Vector3 vec = Vector3.zero;

            while (true)
            {
                Camera_obj.transform.position = Vector3.SmoothDamp(
                    Camera_obj.transform.position,
                    Positions[index],
                    ref vec,
                    0.125f
                );

                float
                    x = Camera_obj.transform.position.x,
                    y = Camera_obj.transform.position.y;
                if (Mathf.Abs(Mathf.Round(x)) >= Mathf.Abs(Positions[index].x) && Mathf.Abs((Mathf.Round(y))) >= Mathf.Abs(Positions[index].y))
                {
                    break;
                }

                yield return null;
            }

            float Timer = 0.0f;
            
            while (true)
            {
                Timer += Time.deltaTime;

                float 
                    curx = Camera_obj.transform.position.x,
                    cury = Camera_obj.transform.position.y;
                Vector3 vect = new Vector3(
                    curx += 10 / Positions[index].x * 0.001f,
                    cury += 5 / Positions[index].y * 0.001f,
                    0
                );
                Camera_obj.transform.position = vect;

                yield return null;
            }
        }

        private IEnumerator Init_T()
        {
            Vector3 vec = Vector3.zero;

            while (true)
            {
                Rings_parent.transform.localScale = Vector3.SmoothDamp(
                    Rings_parent.transform.localScale,
                    new Vector3(2f, 2f, 0),
                    ref vec,
                    0.125f
                );

                yield return null;
            }
        }

        private void init()
        {
            Camera_obj.transform.position = Vector3.zero;
            StartCoroutine(Init_T());
            StartCoroutine(Transform(0));
        }

        private void Start()
        {
            Camera_obj = Camera.gameObject;
            init();
        }
    }
}