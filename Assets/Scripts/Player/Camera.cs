using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class Camera : MonoBehaviour
    {
        internal static object main;
        public Transform playerTransform;
        public float cameraHeight = 8.9f;
        public float cameraDistance = -18.9f;

        private void Start()
        {
            playerTransform = FindObjectOfType<PlayerManager>().transform;
        }

        void Update()
        {
            if (playerTransform == null)
                return;

            transform.position = new Vector3(playerTransform.position.x, cameraHeight, playerTransform.position.z + cameraDistance);
        }
    }
}
