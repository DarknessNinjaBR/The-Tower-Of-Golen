using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class RegenPlayer : MonoBehaviour
    {
        public int heal;

        private void Start()
        {

        }

      
        private void OnTriggerEnter(Collider other)
        {
                PlayerManager player = other.GetComponentInParent<PlayerManager>();

            if (player != null)
            {
                this.GetComponent<Collider>().enabled = false;
                player.Heal(heal);      
            }
        }
    }
}
