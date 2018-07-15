using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmanager : MonoBehaviour {
	public static Soundmanager Instance;

	public AudioClip Laser1;
	public AudioClip Saw1;

	void Awake(){
		Instance = this;
	}
	public void Laser1sound(){
		Makesound (Laser1);
	}
	public void Saw1sound(){
		Makesound (Saw1);
	}
	private void Makesound(AudioClip originalClip){
		AudioSource.PlayClipAtPoint (originalClip, transform.position);
	}
}
