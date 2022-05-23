using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class FireBall : MonoBehaviour
    {
        PlayerManager player;
        Rigidbody rigid;
        public float speed;
        public GameObject explosion;

        public bool isAI = true;

        private void Start()
        {
            rigid = GetComponent<Rigidbody>();

            if (isAI)
            {
                player = FindObjectOfType<PlayerManager>();
                transform.LookAt(player.transform.position);
            }


        }

        private void Update()
        {
            rigid.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }

        private void OnTriggerEnter(Collider other)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
