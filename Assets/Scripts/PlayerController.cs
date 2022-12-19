using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour {

	public int level;
	public float levelTime;
	public TextMeshProUGUI timer;

	private Rigidbody rb;
	private float time;
	private float timerStart;
	private float timeElapsed;
	private float timeRemaining;
	private int minutes;
	private string timerString;
	private bool playerControl = true;
	private bool finished = false;
	private bool dead = false;
	private bool firstFrame = false;

	private void Start() {
		rb = GetComponent<Rigidbody>();

		//Obtient le numéro du niveau
		level = SceneManager.GetActiveScene().buildIndex;

		timerStart = Time.time;

		Debug.Log(SceneManager.sceneCount);
	}

	private void Update() {
		//TODO: Maybe create a save system (but prolly not)
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		UpdateTimer();

		//La fonction elle-même vérifie à seulement un instant, donc elle est insérée dans Update() pour être répétée à chaque frame
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
			}
			time = Time.time;
			finished = true;
		}
	}
	
	private void SceneLoadTimer(float sceneTimer) {

		//Attend soit 5 secondes, ou jusqu'à ce que le joueur appuye un bouton
		if ((Time.time - sceneTimer < 5.0f && Time.time - sceneTimer > 0.05f && Input.anyKeyDown) || (Time.time - sceneTimer >= 5.0f)) {

			SceneManager.LoadSceneAsync(level);
			
		} 
	}

	private void UpdateTimer() {

		if (!finished && (timeElapsed.CompareTo(levelTime) < 0)) {
			timeElapsed = Time.time - timerStart;

			//J'arrondis pour avoir seulement 2 points après la décimale
			timeElapsed = Mathf.Floor(timeElapsed * 100) / 100;

			timeRemaining = levelTime - timeElapsed;
			minutes = (int)timeRemaining / 60;

			//J'utilise la variable firstFrame afin de m'assurer que la variable time est seulement prise une fois, et qu'elle n'est pas mise à jour constamment
		} else if (!firstFrame) {
			dead = true;
			playerControl = false;
			firstFrame = true;
			time = Time.time;
		}

		timerString = string.Format("{0}:{1:00.00}", minutes, timeRemaining % 60);
		timer.text = timerString;
		
	}
}
