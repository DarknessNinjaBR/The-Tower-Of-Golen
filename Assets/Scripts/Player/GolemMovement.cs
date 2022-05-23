using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace TW
{
    public class GolemMovement : MonoBehaviour
    {
        public static Pointer current { get; }


        [Header("Movement")]
        public float moveSpeed = 3f;
        public float rotateSpeed = 5f;
        Rigidbody rigid;
        public bool rotateTowardsMouse;
        public LayerMask layer;

        public float velocity;

        [Header("Fall")]
        public bool isGrounded;
        public bool isInAir;
        public float groundDetectionRayStartPoint = .5f;
        public float minimumDistanceNeededToBeginFall = .5f;
        public LayerMask ignoreForGroundCheck;

        [Header("References")]
        AnimationHandler animationHandler;
        InputManager inputManager;
        CameraHandler camera;
        Camera playerCamera;

        void Start()
        {
            transform.rotation = Quaternion.identity;

            camera = FindObjectOfType<CameraHandler>();
            playerCamera = FindObjectOfType<Camera>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
            inputManager = GetComponent<InputManager>();
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            rigid = GetComponent<Rigidbody>();

        }

        void Update()
        {
            Fall();
            var targetVector = new Vector3(inputManager.horizontal, 0, inputManager.vertical);

            if (animationHandler.isInteracting)
                return;

            var movementVector = MoveTowardTarget(targetVector);
            if (!rotateTowardsMouse)
                RotateTowardsMovementVector(movementVector);
            else
                RotateTowardsMouseVector();
        }

        public void Fall()
        {
            isGrounded = false;
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, -Vector3.up/* + myTransform.position*/, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                isGrounded = true;

                if (isInAir)
                {
                    isInAir = false;
                }
            }
            else
            {
                if (isGrounded)
                {
                    isGrounded = false;
                }

                if (isInAir == false)
                {
                    isInAir = true;
                }
            }
        }

        void RotateTowardsMouseVector()
        {
            Vector3 pos = camera.cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit hit;
            Ray ray = camera.cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                Vector3 worldPosition;
                worldPosition = hit.point;
                worldPosition.y = transform.position.y;
                transform.LookAt(worldPosition);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(worldPosition), rotateSpeed* Time.time);
            }
        }

        void RotateTowardsMovementVector(Vector3 movementVector)
        {
            if(movementVector.magnitude == 0) { return; }

            var rotation = Quaternion.LookRotation(movementVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        }

        private Vector3 MoveTowardTarget(Vector3 targetVector)
        {
            if (animationHandler.isInteracting)
                return Vector3.zero;

            var speed = moveSpeed * Time.deltaTime;

            if (inputManager.dash_Input)
                speed = speed * 100;

            targetVector = Quaternion.Euler(0, playerCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
            var targetPosition = transform.position + targetVector * speed;
            transform.position = targetPosition;
            return targetVector;
        }
    }
}
