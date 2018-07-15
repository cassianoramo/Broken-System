using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Colisor : MonoBehaviour {

	void  OnTriggerEnter2D (Collider2D other){
		if  (other.gameObject.CompareTag ( "Obstaculo")) {
			string currentScene = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene (currentScene);
		}
	}
}
