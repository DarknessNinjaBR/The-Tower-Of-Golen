using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TW
{
    public class Guardian : MonoBehaviour
    {
        public HealthBar healthBar;

        public CutSceneManager cutSceneManager;
        Rigidbody rigidbody;
        Animator animator;
        NavMeshAgent agent;
        public PlayerManager player;

        public GameObject[] damageCollider;

        public int maxHealth = 100;
        public int currentHealth;

        public float rotationSpeed = 7f;
        public float moveSpeed = 3f;
        public float fovRadius = 25;
        public float recoveryTimer;
        LayerMask detectionLayer;

        [HideInInspector]
        public Transform currentTarget;
        bool isInteracting = false;
        bool actionFlag;

        float delta;

        ActionSnapshot currentSnapshot;
        public ActionSnapshot[] actionSnapshots;

        public Vector3 deltaPosition;

        public bool lookAtPlayer;
        public bool canRotate;
        public bool canMove;
        public bool hasLookAtTarget;
        public Vector3 lookAtPosition;

        public bool isHit;
        float hitTimer;
        public float stunTimer;

        

        public ActionSnapshot GetAction(float distance, float angle)
        {
            int maxScore = 0;
            for (int i = 0; i < actionSnapshots.Length; i++)
            {
                ActionSnapshot a = actionSnapshots[i];

                if (distance <= a.maxDist && distance >= a.minDist)
                {
                    if (angle <= a.maxAngle && angle >= a.minAngle)
                    {
                        maxScore += a.score;
                    }
                }
            }

            int ran = Random.Range(0, maxScore + 1);
            int temp = 0;

            for (int i = 0; i < actionSnapshots.Length; i++)
            {
                ActionSnapshot a = actionSnapshots[i];

                if (a.score == 0)
                    continue;

                if (distance <= a.maxDist && distance >= a.minDist)
                {
                    if (angle <= a.maxAngle && angle >= a.minAngle)
                    {
                        temp += a.score;
                        if (temp > ran)
                        {
                            return a;
                        }
                    }
                }
            }
            return null;
        }


        private void Start()
        {
            cutSceneManager = FindObjectOfType<CutSceneManager>();
            currentHealth = maxHealth;

            healthBar.SetBar(currentHealth);

            player = FindObjectOfType<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f,0);
        }

        public bool isWater = false;

        private void Update()
        {

            if (player == null)
            {
                agent.destination = this.transform.position;
                return;
            }

            if (transform.position.y < 0 && !isWater)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }

            animator.applyRootMotion = isInteracting;
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting");

            if (stunTimer > 0)
            {
                stunTimer -= delta;
            }

            if (isHit)
            {
                if (hitTimer > 0)
                {
                    hitTimer -= delta;
                }
                else
                {
                    isHit = false;
                }
            }


            if (!isInteracting)
            {
                hitted = false;
                if (actionFlag)
                {
                    recoveryTimer -= delta;
                    if (recoveryTimer <= 0)
                    {
                        actionFlag = false;
                    }
                }

                if (lookAtPlayer)
                {
                    animator.transform.LookAt(currentTarget);
                }

                currentTarget = player.transform;
                Vector3 relativeVelocity = transform.InverseTransformDirection(agent.desiredVelocity);
                animator.SetFloat("movement", relativeVelocity.z, 0.1f, Time.deltaTime);

                Vector3 dir = currentTarget.transform.position - transform.position;
                dir.y = 0;
                dir.Normalize();

                float dis = Vector3.Distance(transform.position, currentTarget.transform.position);
                float angle = Vector2.Angle(transform.position, dir);
                float dot = Vector3.Dot(transform.right, dir);
                if (dot < 0)
                    angle *= -1;

                currentSnapshot = GetAction(dis, angle);

                if (currentSnapshot != null && actionFlag == false)
                {
                    // HandleRotation(delta);
                    PlayTargetAnimation(currentSnapshot.anim, true);
                    actionFlag = true;
                    recoveryTimer = currentSnapshot.recoveryTime;
                }
                else
                {
                    //animator.SetFloat("sideways", 0, 0.1f, delta);

                }
            }

            if (!isInteracting)
            {
                
                agent.speed = moveSpeed;

                float dis = Vector3.Distance(transform.position, currentTarget.transform.position);
                if (dis < 2)
                {
                    float rotationLess = rotationSpeed / 4;
                    HandleRotation(delta, rotationLess);

                }
            }

            if (isInteracting)
            {
                agent.speed = 0;
                if (canRotate)
                    HandleRotation(delta, rotationSpeed);

                animator.SetFloat("movement", 0, 0.1f, delta);
                //animator.SetFloat("sideways", 0, 0.1f, delta);
                Vector3 targetPos = animator.transform.position;
                targetPos.y = 0;

                transform.position = animator.transform.position;
                transform.rotation = animator.transform.rotation;

                animator.transform.localPosition = Vector3.zero;
                animator.transform.localRotation = Quaternion.identity;

            }


            Vector3 targetVel = deltaPosition * moveSpeed;
            rigidbody.velocity = targetVel;
            agent.destination = currentTarget.position;

        }

       

        void HandleRotation(float delta, float speed)
        {
            Vector3 dir = currentTarget.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();

            if (dir == Vector3.zero)
            {
                dir = transform.forward;
            }

            float angle = Vector3.Angle(dir, transform.forward);
            if (angle > 20)
            {
                //animator.SetFloat("sideways", Vector3.Dot(dir, mTransform.right), 0.1f, delta);
            }

            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, delta * speed);
        }
        float lastDamage = 0;
        public float damageHate = 3;
        public bool hitted;


        [Header("Multipliers")]
        public float fireMultiplier = 1;
        public float waterMultiplier = 1;
        public float stoneMultiplier = 1;
        public float poisonMultiplier = 1;
        public float plantMultiplier = 1;

        public void Damage(int damage, string type)
        {
            float currentMultiplier = 1;
            switch (type)
            {
                case "Fire":
                    currentMultiplier = fireMultiplier;
                    break;
                case "Water":
                    currentMultiplier = waterMultiplier;
                    break;
                case "Stone":
                    currentMultiplier = stoneMultiplier;
                    break;
                case "Poison":
                    currentMultiplier = poisonMultiplier;
                    break;
                case "Plant":
                    currentMultiplier = plantMultiplier;
                    break;
                default:
                    currentMultiplier = 1;
                    break;
            }

            int totalDamage = (int)(damage * currentMultiplier);

            currentHealth -= totalDamage;
            healthBar.UpdateBar(currentHealth);
            if (currentHealth > 0)
            {
                if (Time.time > damageHate + lastDamage && totalDamage > 0.1)
                {
                    PlayTargetAnimation("Hitted", true);
                    lastDamage = Time.time;
                    hitted = true;
                }
            }
            else
            {
                PlayTargetAnimation("Dead", true);
                currentTarget = this.transform;
                hitted = true;
                cutSceneManager.Dead();

                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        [System.Serializable]
        public class ActionSnapshot
        {
            public string anim;
            public int score = 5;
            public float recoveryTime;
            public float minDist = 2;
            public float maxDist = 5;
            public float minAngle = -35;
            public float maxAngle = 35;
        }
    }
}
