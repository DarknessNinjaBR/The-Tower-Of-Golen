using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW {
    public class DamagePlayer : MonoBehaviour
    {
        public bool oneHit;
        public bool areaHit;

        public bool damageIA;

        public float damageDelay = .5f;

        public int damage;

        private void Start()
        {
            damageDelayTimer = damageDelay;
            damageDelayTimer = damageDelay + 1;
        }

        private void Update()
        {
            if (damageDelayTimer < damageDelay)
            {
                damageDelayTimer += Time.deltaTime;
            }
        }
        bool ativo;
        private void OnEnable()
        {
            ativo = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (areaHit)
                return;

            if (!damageIA)
            {

                PlayerManager player = other.GetComponentInParent<PlayerManager>();

                if (player != null)
                {
                    if (ativo)
                    {
                        player.Damage(damage);
                        damageDelayTimer = 0;
                        ativo = false;
                    }
                }
            }
            else
            {
                Guardian guardian = other.GetComponent<Guardian>();

                if (guardian != null)
                {
                    if (ativo)
                    {
                        guardian.Damage(damage, this.tag);
                        damageDelayTimer = 0;
                        ativo = false;

                    }
                }

            }

        }
        float damageDelayTimer;
        private void OnTriggerStay(Collider other)
        {
            if (oneHit)
                return;


            if (!damageIA)
            {

                PlayerManager player = other.GetComponentInParent<PlayerManager>();

                if (player != null)
                {
                    if (damageDelayTimer >= damageDelay)
                    {
                        player.Damage(damage);
                        damageDelayTimer = 0;
                    }
                }
            }
            else
            {
                Guardian guardian = other.GetComponent<Guardian>();

                if (guardian != null)
                {
                    if (damageDelayTimer >= damageDelay)
                    {
                        guardian.Damage(damage, this.tag);
                        damageDelayTimer = 0;
                    }
                }
            }


        }
    }
}
