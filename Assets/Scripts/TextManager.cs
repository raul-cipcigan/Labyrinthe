using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour {
	public TextMeshProUGUI level1;

	private void Start() {
		ResetText();
	}

	public void ResetText() {
		level1.text = string.Format("{0}:{1:00.00}", (int)PlayerPrefs.GetFloat("level1time") / 60, PlayerPrefs.GetFloat("level1time") % 60);
	}
}
