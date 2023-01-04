using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour {
    public TextManager textManager;
    public GameObject[] button = new GameObject[5];

	private void Start() {
        SetLevelProgress();
	}

	private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void OnLevelButtonPress(int level) {

        if (level == 0) {
            Application.Quit();
        } else {
            SceneManager.LoadSceneAsync(level);
        }
	}

    public void OnResetButtonPress() {
        PlayerPrefs.DeleteAll();
        textManager.ResetText();
        SetLevelProgress();
	}

    private void SetLevelProgress() {
        for (int i = 1; i <= (button.Length - 1); i++) {
            if (PlayerPrefs.HasKey("level" + (i + 1))) {
                button[i].SetActive(true);
            } else {
                button[i].SetActive(false);
			}
        }
    }
}
