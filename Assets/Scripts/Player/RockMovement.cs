using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class RockMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 50f;
        public float dashDistance = 100f;
        public float jumpStrenght = 5f;
        public float dashTimer = 1.3f;
        Rigidbody rigid;

        public float velocity;

        [Header("Fall")]
        public bool isGrounded;
        public bool isInAir;
        public float groundDetectionRayStartPoint = .5f;
        public float minimumDistanceNeededToBeginFall = .5f;
        LayerMask ignoreForGroundCheck;

        [Header("References")]
        InputManager inputManager;
        public HealthBar staminaBar;

        public AudioSource audioDamage;

        void Start()
        {
            inputManager = GetComponent<InputManager>();
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            rigid = GetComponent<Rigidbody>();
            staminaBar.SetBar((int)dashTimer);

            dashTimerCount = dashTimer + 1;
        }

        void Update()
        {
            velocity = rigid.velocity.magnitude;
            Movement(inputManager.delta);
            Fall();
        }

        private void FixedUpdate()
        {
            
        }

        public float dashTimerCount;
        public void Movement(float delta)
        {
            Vector3 velocity = new Vector3(inputManager.vertical, 0, -inputManager.horizontal);
            rigid.AddTorque(velocity * speed * delta * 10);

            if (inputManager.dash_Input && dashTimerCount > dashTimer)
            {
                Dash(delta);
            }

            if (dashTimerCount < dashTimer)
            {
                dashTimerCount += Time.deltaTime;
                staminaBar.UpdateBar((int)dashTimerCount);
            }

            if (inputManager.jump_Input && isGrounded)
            {
                rigid.AddForce(Vector3.up * jumpStrenght, ForceMode.Impulse);
                Debug.Log(Vector3.up * jumpStrenght);
            }
        }

        public void Dash(float delta) {
            dashTimerCount = 0;
            Vector3 dashVelocity = new Vector3(inputManager.horizontal, 0, inputManager.vertical);
            rigid.AddForce(dashVelocity * dashDistance, ForceMode.Impulse);
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

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy" && (int)velocity > 8)
            {
                other.GetComponent<Guardian>().Damage((int)(velocity/1.5f), "Stone");
                audioDamage.Play();
                Vector3 moveDir;
                moveDir = rigid.transform.position - other.transform.position;
                rigid.AddForce(moveDir.normalized * 5.5f, ForceMode.Impulse);
            }
        }

    }
}
