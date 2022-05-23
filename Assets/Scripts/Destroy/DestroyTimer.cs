using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class DestroyTimer : MonoBehaviour
    {
        public bool useTimer = true;
        public bool enemyDestroys;
        public bool playerDestroys;
        public bool useParticle = false;
        public GameObject particle;
        public float timer;

        private void Start()
        {
            if (useTimer && useParticle)
                StartCoroutine(TimerCreate());

        }

        private void Update()
        {

            if (useTimer)
            {
                Destroy(gameObject, timer);              
                //Instantiate(particle, transform.position, transform.rotation);
            }
        }

        IEnumerator TimerCreate()
        {
            yield return new WaitForSeconds(timer - .01f);
            Instantiate(particle, transform.position, transform.rotation);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (playerDestroys)
            {

                PlayerManager player = other.GetComponentInParent<PlayerManager>();

                if (player != null)
                {
                    if (useParticle)
                        Instantiate(particle, transform.position, transform.rotation);

                    Destroy(gameObject);
                }
            }
            else if (enemyDestroys)
            {
                Guardian guardian = other.GetComponent<Guardian>();

                if (guardian != null)
                {
                    if (useParticle)
                        Instantiate(particle, transform.position, transform.rotation);

                    Destroy(gameObject);
                }
            }
        }


    }

}
