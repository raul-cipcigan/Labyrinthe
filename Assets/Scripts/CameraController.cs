using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private float maxRadius = 1.5f;
	private Vector3 offset;
	private Vector3 camPos;
	private float interpolant;
	private float currentRadius;

	private void Start() {
		offset = transform.position - player.transform.position;
	}

	private void LateUpdate() {

		//Utilise théorême de Pythagore pour trouver si le point de vue de la caméra se retrove dans un certain cercle autour de la balle
		currentRadius = Mathf.Pow(player.transform.position.x + offset.x - transform.position.x, 2.0f) + Mathf.Pow(player.transform.position.z + offset.z - transform.position.z, 2.0f);

		if (currentRadius > Mathf.Pow(maxRadius, 2.0f)) {

			//Trouve le pourcentage du distance entre la position centrale de la caméra et sa position actuelle
			interpolant = Mathf.Pow(maxRadius, 2.0f) / currentRadius;
			interpolant = 1.0f - interpolant;

			//Déplace la caméra à cette position
			camPos = Vector3.Lerp(transform.position, player.transform.position + offset, interpolant);
			camPos.y = player.transform.position.y + offset.y;
			transform.position = camPos;

		} else {

			//Utilise Vector3.Lerp afin de recentrer la caméra à chaque seconde
			camPos = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime);
			camPos.y = player.transform.position.y + offset.y;
			transform.position = camPos;

		}

	}

}