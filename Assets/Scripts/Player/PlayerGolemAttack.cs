using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
    public class PlayerGolemAttack : MonoBehaviour
    {
        public AnimationHandler animationHandler;
        InputManager inputManager;


        public string[] attacks;
        public string[] specialAttacks;
        public GameObject[] gemObj;
        public GameObject[] weaponObject;
        public GameObject[] damageColliderF;
        public GameObject[] damageColliderW;
        public GameObject[] damageColliderS;
        public GameObject[] damageColliderPo;
        public GameObject[] damageColliderPl;
        int index = 0;
        public int maxStamina = 100;
        public int stamina;
        public HealthBar staminaBar;

        private void Start()
        {
            stamina = maxStamina;
            staminaBar.SetBar(stamina);

            animationHandler = GetComponentInChildren<AnimationHandler>();
            inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            if (stamina > 0)
                HandleAttack();

            HandleInventory();
            animationHandler.gameObject.transform.localPosition = Vector3.zero;
            animationHandler.gameObject.transform.localRotation = Quaternion.identity;

            if (animationHandler.isInteracting)
            {
                weaponObject[index].SetActive(true);
            }
            else
            {
                weaponObject[index].SetActive(false);
            }
        }
        private void FixedUpdate()
        {
            if (!animationHandler.isInteracting)
                stamina += 1;
        }

        public void HandleInventory()
        {
            if (inputManager.rightInv_Input && !animationHandler.isInteracting)
            {
                gemObj[index].SetActive(false);
                index++;
                if (index > gemObj.Length - 1)
                    index = 0;

                gemObj[index].SetActive(true);


            }
            else if (inputManager.leftInv_Input && !animationHandler.isInteracting)
            {
                gemObj[index].SetActive(false);
                index--;
                if (index < 0)
                    index = gemObj.Length - 1;
                gemObj[index].SetActive(true);
            }
        }


        void HandleAttack()
        {
            Shot();
            SpecialShot();

            if (animationHandler.isInteracting)
            {
                transform.position = animationHandler.gameObject.transform.position;
                transform.rotation = animationHandler.gameObject.transform.rotation;
            }
        }

        public void Shot()
        {
            if (inputManager.cast_Input && !animationHandler.isInteracting)
            {
                animationHandler.PlayTargetAnimation(attacks[index], true);
                stamina -= 25;
                staminaBar.UpdateBar(stamina);

            }
        }
        public void SpecialShot()
        {
            if (inputManager.specialCast_Input && !animationHandler.isInteracting)
            {
                animationHandler.PlayTargetAnimation(specialAttacks[index], true);
                stamina -= 75;
                staminaBar.UpdateBar(stamina);
            }
        }

    }
}