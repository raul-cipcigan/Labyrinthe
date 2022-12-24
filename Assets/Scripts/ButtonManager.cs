using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    public TextManager textManager;

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
	}
}
