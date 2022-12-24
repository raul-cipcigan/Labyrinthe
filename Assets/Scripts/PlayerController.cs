using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour {

	public float levelTime;
	public TextMeshProUGUI timer;
	public TextMeshProUGUI winStatus;
	public TextMeshProUGUI winTimer;

	private Rigidbody rb;

	private float time;
	private float timerStart;
	private float timeElapsed;
	private float timeRemaining;
	private float bestTime;
	private int minutes;
	private string timerString;

	private bool playerControl = true;
	private bool finished = false;
	private bool dead = false;
	private bool firstFrame = true;

	private int level;

	private void Start() {
		rb = GetComponent<Rigidbody>();

		//Obtient le num�ro du niveau
		level = SceneManager.GetActiveScene().buildIndex;

		timerStart = Time.time;

		winStatus.text = "";
		winTimer.text = "";

		if (PlayerPrefs.GetInt("level" + level.ToString()) != 1) {
			PlayerPrefs.SetInt("level" + level.ToString(), 1);
			PlayerPrefs.Save();
		}

	}

	private void Update() {
		//TODO: Maybe create a save system (but prolly not)
		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadSceneAsync(0);
		}

		UpdateTimer();

		//La fonction elle-m�me v�rifie � seulement un instant, donc elle est ins�r�e dans Update() pour �tre r�p�t�e � chaque frame
		if (finished) {
			SceneLoadTimer(time);
		} else if (dead) {
			SceneLoadTimer(time);
		}

	}

	private void FixedUpdate() {
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		Vector3 mov = new Vector3(xAxis, 0.0f, yAxis);

		if (playerControl) {
			rb.AddForce(mov * 10);
		}
	}

	private void OnTriggerEnter(Collider other) {
		
		//Si le joueur touch un objet "Finish", le prochain niveau est ouvert
		if (other.gameObject.CompareTag("Finish")) {
			playerControl = false;
			//S'assurer qu'un joueur qui perd ne peut pas rentrer dans le but
			if (!dead) {
				level++;
				finished = true;
				time = Time.time;
			}
		}
	}
	
	private void SceneLoadTimer(float sceneTimer) {

		if (dead) {
			winStatus.text = "Perdu...";
		} else {

			if (!PlayerPrefs.HasKey("level" + (level - 1).ToString() + "time")) {

				PlayerPrefs.SetFloat("level" + (level - 1).ToString() + "time", timeElapsed);

			//Comparer le temps pr�c�dent avec le nouveau temps
			} else if (timeElapsed < PlayerPrefs.GetFloat("level" + (level - 1).ToString() + "time")) {

				PlayerPrefs.SetFloat("level" + (level - 1).ToString() + "time", timeElapsed);

			}

			bestTime = PlayerPrefs.GetFloat("level" + (level - 1).ToString() + "time");

			winStatus.text = "Gagn�!";
			winTimer.text = string.Format("Compl�t� en: {0}:{1:00.00}\nMeilleur temps: {2}:{3:00.00}", (int)timeElapsed / 60, timeElapsed % 60, (int)bestTime / 60, bestTime % 60);
		}

		//Attend soit 5 secondes, ou jusqu'� ce que le joueur appuye un bouton
		if ((Time.time - sceneTimer < 5.0f && Time.time - sceneTimer > 0.05f && Input.anyKeyDown) || (Time.time - sceneTimer >= 5.0f)) {

			SceneManager.LoadSceneAsync(level);
			
		} 
	}

	private void UpdateTimer() {

		if (!finished && (timeElapsed.CompareTo(levelTime) < 0)) {
			timeElapsed = Time.time - timerStart;

			//J'arrondis pour avoir seulement 2 points apr�s la d�cimale
			timeElapsed = Mathf.Floor(timeElapsed * 100) / 100;

			timeRemaining = levelTime - timeElapsed;
			minutes = (int)timeRemaining / 60;

			//J'utilise la variable firstFrame afin de m'assurer que la variable time est seulement prise une fois, et qu'elle n'est pas mise � jour constamment
		} else if (!finished && firstFrame) {
			dead = true;
			playerControl = false;
			firstFrame = false;
			time = Time.time;
		}

		timerString = string.Format("{0}:{1:00.00}", minutes, timeRemaining % 60);
		timer.text = timerString;
		
	}
}
