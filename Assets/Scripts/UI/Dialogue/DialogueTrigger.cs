using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
	public class DialogueTrigger : MonoBehaviour
	{

		public Dialogue dialogue;

		public void TriggerDialogue()
		{
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		}

	}
}