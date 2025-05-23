/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    // Let the camera follow the target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.2f;  
        private Vector3 velocity = Vector3.zero; 

        private Vector3 offset;
        private Vector3 targetPos;

        public Vector2 center;
        public Vector2 mapSize;

        private float camWidth;
        private float camHeight;

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;

            camHeight = Camera.main.orthographicSize;
            camWidth = camHeight * Screen.width / Screen.height;
        }

        private void LateUpdate() 
        {
            if (target == null) return;

            targetPos = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            float limitX = mapSize.x - camWidth;
            float clampX = Mathf.Clamp(transform.position.x, -limitX + center.x, limitX + center.x);

            float limitY = mapSize.y - camHeight;
            float clampY = Mathf.Clamp(transform.position.y, -limitY + center.y, limitY + center.y);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.2f;
        private Vector2 velocity = Vector2.zero;

        private Vector2 offset;
        private Vector2 targetPos;

        private void Start()
        {
            if (target == null) return;

            offset = (Vector2)transform.position - (Vector2)target.position;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            targetPos = (Vector2)target.position + offset;
            Vector2 smoothedPosition = Vector2.SmoothDamp((Vector2)transform.position, targetPos, ref velocity, smoothTime);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }

    }
}
