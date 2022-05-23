using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW {
    public class CutSceneManager : MonoBehaviour
    {
        public DialogueTrigger dialogueTrigger;

        public DialogueManager dialogueManager;

        public PauseMenu pauseMenu;

        public GameObject dialogueObj;
        public GameObject hudObj;

        public Guardian[] guardian;
        public RockMovement rockMovement;
        public Rigidbody rigidbody;

        public AudioSource audio;
        public AudioClip dialogueMusic;
        public AudioClip bossMusic;

        public Muzzle muzzle;

        int guardianQnt;
        private void Start()
        {
            for (int i = 0; i < guardian.Length; i++)
            {
                guardian[i].enabled = false;
            }
            rockMovement.enabled = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            if(muzzle != null)
                muzzle.enabled = false;

            pauseMenu = FindObjectOfType<PauseMenu>();
            if(dialogueTrigger != null)
                dialogueTrigger.TriggerDialogue();

            if (audio != null && dialogueTrigger != null)
            {
                audio.Stop();
                audio.clip = dialogueMusic;
                audio.Play();
            }

            if(guardian != null)
                guardianQnt = guardian.Length;


            if(hudObj != null)
                hudObj.SetActive(false);
        }
        int count = 0;

        public int sceneIndex;
        public void Dead()
        {
            count++;
            Debug.Log(count);
            if(count >= guardianQnt)
            {
                pauseMenu.Vitoria(sceneIndex);
            }
        }

        public void StartGame()
        {
            for (int i = 0; i < guardian.Length; i++)
            {
                guardian[i].enabled = true;
            }
            rockMovement.enabled = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            if (muzzle != null)
                muzzle.enabled = true;
            hudObj.SetActive(true);
            audio.Stop();
            audio.clip = bossMusic;
            audio.Play();

        }
        

    }
}