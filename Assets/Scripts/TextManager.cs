using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour {
	public TextMeshProUGUI[] level = new TextMeshProUGUI[5];

	private void Start() {
		ResetText();
	}

	public void ResetText() {
		string key;

		//Pour chaque niveau, on verifie si un meilleur temps existe, et s'il n'existe pas, on montre DNF
		for (int i = 0; i < 5; i++) {

			key = "level" + (i + 1).ToString() + "time";

			if (!PlayerPrefs.HasKey(key)) {
				level[i].text = "DNF";
			} else {
				level[i].text = string.Format("{0}:{1:00.00}", (int)PlayerPrefs.GetFloat(key) / 60, PlayerPrefs.GetFloat(key) % 60);
			}
		}
	}
}