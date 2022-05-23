using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class PlayerManager : MonoBehaviour
    {
        public int maxLife;
        public int currentLife;
        public HealthBar healthBar;
        public AudioSource damageSound;
        private void Start()
        {
            currentLife = maxLife;

            healthBar.SetBar(currentLife);
        }

        public void Damage(int damage)
        {
            currentLife -= damage;
            healthBar.UpdateBar(currentLife);
            damageSound.Play();

            if (currentLife < 1)
            {
                Destroy(gameObject);
            }
        }
        public void Heal(int heal)
        {
            currentLife += heal;
            if (currentLife > maxLife)
                currentLife = maxLife;
            healthBar.UpdateBar(currentLife);
        }

    }
}
