using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace TW
{
    public class Muzzle : MonoBehaviour {

        public static Pointer current { get; }

        PlayerManager player;
        CameraHandler a;
        InputManager inputManager;

        public GameObject aim;

        public Transform muzzleTransform;
        public GameObject[] spellObj;
        public GameObject[] specialSpellObj;
        public GameObject[] gemObj;
        public Sprite[] hudIcon;
        public Image hudImage;
        public bool[] usedSpecialSpell;
        int index = 0;
        public float fireRate = 0.5f;
        float lastShot = 0;


        private Vector3 inputRotation;
        private Vector3 mousePlacement;
        private Vector3 screenCentre;

        public Vector3 worldPosition;
        public LayerMask layer;
        public MeshCollider plane;

        private void Start()
        {
            hudImage.sprite = hudIcon[0];
            usedSpecialSpell = new bool[specialSpellObj.Length];

            inputManager = FindObjectOfType<InputManager>();
            player = FindObjectOfType<PlayerManager>();
            a = GetComponent<CameraHandler>();

            lastShot = Time.time;
        }

         void Update()
         {
            float analogAmout = Mathf.Clamp01(Mathf.Abs(inputManager.analogInput.x) + Mathf.Abs(inputManager.analogInput.y));

            if (analogAmout > 0)
            {
                HandleAnalog();
                aim.SetActive(true);
            }
            else
            {
                HandleAim();
                aim.SetActive(false);
            }

            HandleInventory();
         }

        public void HandleAim()
        {
            //Mouse.current.position

            //Vector3 worldPos = a.cam.ScreenToWorldPoint(Mouse.current.position);
            Vector3 pos = a.cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit hit;
            Ray ray = a.cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                worldPosition = hit.point;
                transform.LookAt(worldPosition);
                Shot();
                SpecialShot();
            }


            //Quaternion targetRotation = Quaternion.LookRotation(inputManager.analogInput, Vector3.back);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }

        public void HandleAnalog()
        {
            //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputManager.analogInput.x, inputManager.analogInput.y, 0), Vector3.back);

            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);

            Vector3 lookDirection = new Vector3(inputManager.analogInput.x, 0, inputManager.analogInput.y);
            transform.rotation = Quaternion.LookRotation(lookDirection);

            Shot();
            SpecialShot();
        }

        public void HandleInventory()
        {
            if (inputManager.rightInv_Input)
            {
                gemObj[index].SetActive(false);
                index++;
                if (index > gemObj.Length-1)
                    index = 0;

                gemObj[index].SetActive(true);
                hudImage.sprite = hudIcon[index];


            }
            else if (inputManager.leftInv_Input)
            {
                gemObj[index].SetActive(false);
                index--;
                if (index < 0)
                    index = gemObj.Length-1;
                gemObj[index].SetActive(true);
                hudImage.sprite = hudIcon[index];
            }
        }

        public void Shot()
        {
            if (inputManager.cast_Input && Time.time > fireRate + lastShot)
            {
                Instantiate(spellObj[index], muzzleTransform.position, muzzleTransform.rotation);
                lastShot = Time.time;
            }
        }
        public void SpecialShot()
        {
            if (inputManager.specialCast_Input && !usedSpecialSpell[index])
            {
                Instantiate(specialSpellObj[index], transform.position, transform.rotation);
                usedSpecialSpell[index] = true;
            }
        }

        private void LateUpdate()
        {
            if (player == null)
            {
                Destroy(gameObject,0.1f);
                return;
            }

            transform.position = player.transform.position + new Vector3(0,1.23f,0);
        }
    }
}