using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace TW
{
	public class DialogueManager : MonoBehaviour
	{

		public TMP_Text nameText;
		public TMP_Text dialogueText;
		public Image icon;

		public Animator animator;

		public CutSceneManager CutSceneManager;

		private Queue<string> sentences;

		void Awake()
		{
			sentences = new Queue<string>();
		}

		public void StartDialogue(Dialogue dialogue)
		{
			animator.SetBool("IsOpen", true);

			nameText.text = dialogue.name;
			icon.sprite = dialogue.icon;

			sentences.Clear();

			foreach (string sentence in dialogue.sentences)
			{
				sentences.Enqueue(sentence);
			}

			DisplayNextSentence();
		}

		public void DisplayNextSentence()
		{
			if (sentences.Count == 0)
			{
				EndDialogue();
				return;
			}

			string sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}

		IEnumerator TypeSentence(string sentence)
		{
			dialogueText.text = "";
			foreach (char letter in sentence.ToCharArray())
			{
				dialogueText.text += letter;
				yield return null;
			}
		}

        public DialogueTrigger[] newDialogue;
        int i = 0;

        public bool newDialogueBool;
        public bool isPlant = false;
		void EndDialogue()
		{
			animator.SetBool("IsOpen", false);

            if (isPlant)
            {
                FindObjectOfType<CutSceneManager>().Dead();
                return;
            }


            if (newDialogueBool && this.i < newDialogue.Length)
            {

                newDialogue[this.i].TriggerDialogue();
                this.i++;
                if (this.i > newDialogue.Length)
                    CutSceneManager.StartGame();

            }
            else
            {
                CutSceneManager.StartGame();
            }
		}

	}
}