using System.Collections;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerWarp : MonoBehaviour
    {
        public string warpTriggerTag = "WarpZone";
        private CameraFollow cameraFollow;
        private Coroutine warpCoroutine;  // 코루틴 저장용
        private bool isWarping = false;

        private void Start()
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(warpTriggerTag))
            {
                WarpZone warpZone = collision.GetComponent<WarpZone>();
                if (warpZone != null && warpZone.targetPoint != null)
                {
                    // 중복 방지
                    if (warpCoroutine == null)
                    {
                        warpCoroutine = StartCoroutine(WarpAfterDelay(warpZone.targetPoint));
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(warpTriggerTag))
            {
                if (warpCoroutine != null)
                {
                    StopCoroutine(warpCoroutine);
                    warpCoroutine = null;
                    isWarping = false;
                    Debug.Log("Warp cancelled!");
                }
            }
        }

        private IEnumerator WarpAfterDelay(Transform target)
        {
            isWarping = true;
            yield return new WaitForSeconds(1.5f);

            transform.position = target.position;

            if (cameraFollow != null)
            {
                cameraFollow.transform.position = new Vector3(
                    target.position.x, target.position.y, cameraFollow.transform.position.z);
            }

            warpCoroutine = null;
            isWarping = false;
        }
    }
}
