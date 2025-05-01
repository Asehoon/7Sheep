using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerWarp : MonoBehaviour
    {
        public Transform spawnPointParent;
        private Transform[] spawnPoints;
        private int currentSpawnIndex = 0;
        public string warpTriggerTag = "WarpZone";
        private CameraFollow cameraFollow;

        private void Start()
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();

            if (spawnPointParent != null)
            {
                var childCount = spawnPointParent.childCount;
                spawnPoints = new Transform[childCount];

                int index = 0;
                foreach (var child in spawnPointParent)
                    spawnPoints[index++] = (Transform)child;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(warpTriggerTag))
                StartCoroutine(WarpWithFade());
        }

        private IEnumerator WarpWithFade()
        {
            if (spawnPoints == null || spawnPoints.Length == 0) yield break;

            currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;
            Vector3 nextPos = spawnPoints[currentSpawnIndex].position;
            transform.position = nextPos;

            if (cameraFollow != null)
            {
     
                cameraFollow.transform.position = new Vector3(nextPos.x, nextPos.y, cameraFollow.transform.position.z);
            }

        }

        
    }
}
