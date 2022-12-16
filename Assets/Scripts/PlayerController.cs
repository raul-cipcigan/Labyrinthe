using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	private int level;
	private float time;
	private bool playerControl;
	private bool finished = false;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		playerControl = true;
		level = SceneManager.GetActiveScene().buildIndex;
	}

    private void Update() {
		//TODO: Maybe create a save system (but prolly not)
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
        }

		if (SceneManager.sceneCount == 2) {

			//Afin de s'assurer qu'il n'y a pas trop d'objets dans le jeu en même temps
			SceneManager.UnloadSceneAsync(level - 1);

		}

		if (finished) {
			NextLevelTimer();
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
			level++;
			time = Time.time;
			finished = true;
		}
    }
	
	private void NextLevelTimer() {

		//Attend soit 5 secondes, ou jusqu'à ce que le joueur appuye un bouton
		if ((Time.time - time < 5.0f && Time.time - time > 0.05f && Input.anyKeyDown) || (Time.time - time >= 5.0f)) {

			SceneManager.LoadScene(level);
       
		} 
    }
}
