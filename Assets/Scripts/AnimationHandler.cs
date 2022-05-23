using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW {
    public class AnimationHandler : MonoBehaviour
    {
        public Animator anim;
        public bool isInteracting;

        public Guardian guardian;
        public PlayerGolemAttack player;
        public GameObject wall;
        public GameObject particle;
        public GameObject ground;
        public GameObject fireBall;
        public GameObject waterBall;
        public Transform ballCast;
        public Transform wallCast;
        public bool isIA;

        public bool sound = false;

        public Transform[] waterGuardionCast;

        private void Start()
        {
            if(guardian == null && isIA)
            {
                guardian = GetComponentInParent<Guardian>();
            }
            if (player == null && !isIA)
            {
                player = GetComponentInParent<PlayerGolemAttack>();
            }
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            isInteracting = anim.GetBool("isInteracting");
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void FireBall()
        {
            if (!guardian.hitted)
            {
                if(fireBall != null)
                    Instantiate(fireBall, ballCast.position, ballCast.rotation);

            }
        }

        public void Wall()
        {
            if (!guardian.hitted)
            {
                if (wall != null)
                    Instantiate(wall, wallCast.position, wallCast.rotation);
            }
        }

        public void OpenDamage(int index)
        {
            if (guardian != null && index < guardian.damageCollider.Length)
            {
                if (!guardian.hitted)
                    guardian.damageCollider[index].SetActive(true);
            }
            if (player != null && index < player.damageColliderF.Length)
            {
                player.damageColliderF[index].SetActive(true);
                player.damageColliderW[index].SetActive(true);
                player.damageColliderS[index].SetActive(true);
                player.damageColliderPo[index].SetActive(true);
                player.damageColliderPl[index].SetActive(true);
            }
        }

        public void CloseDamage(int index)
        {
            if (guardian != null && index < guardian.damageCollider.Length)
                guardian.damageCollider[index].SetActive(false);

            if (player != null && index < player.damageColliderF.Length)
            {
                player.damageColliderF[index].SetActive(false);
                player.damageColliderW[index].SetActive(false);
                player.damageColliderS[index].SetActive(false);
                player.damageColliderPo[index].SetActive(false);
                player.damageColliderPl[index].SetActive(false);
            }
        }

        public void CanMove()
        {
            if(guardian != null)
                guardian.currentTarget = guardian.player.transform;
        }

        public void CanNotMove()
        {
            if (guardian != null)
                guardian.currentTarget = guardian.transform;
        }

        public void Particles(int value)
        {
            if (!guardian.hitted)
            {
                if (particle != null)
                    Instantiate(particle, guardian.damageCollider[value].transform.position, guardian.damageCollider[value].transform.rotation);
                if (ground != null)
                    Instantiate(ground, guardian.damageCollider[value].transform.position, guardian.damageCollider[value].transform.rotation);
            }
        }
        public void ParticleExplostion(int value)
        {
            if (!guardian.hitted)
            {
                if (particle != null)
                    Instantiate(particle, guardian.damageCollider[value].transform.position, guardian.damageCollider[value].transform.rotation);
            }
        }

        public void ParticlesGround(int value)
        {
            if (!guardian.hitted)
            {
                if (ground != null)
                    Instantiate(ground, guardian.damageCollider[value].transform.position, guardian.damageCollider[value].transform.rotation);
            }
        }

        public void WaterCast()
        {
            if(!guardian.hitted)
                for (int i = 0; i < waterGuardionCast.Length; i++)
                {
                    Instantiate(waterBall, waterGuardionCast[i].position, waterGuardionCast[i].rotation);
                }
        }

    }
}