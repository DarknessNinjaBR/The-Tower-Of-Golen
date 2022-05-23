using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW
{
	[System.Serializable]
	public class Dialogue
	{
		public Sprite icon;
		public string name;

		[TextArea(3, 10)]
		public string[] sentences;

	}
}