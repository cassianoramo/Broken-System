using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	private Animator anim;
	private Rigidbody2D rb2d;
	public Transform posPe;
	[HideInInspector] public bool tocaChao = false;
	public float Velocidade;
	public float ForcaPulo = 1000f;
	[HideInInspector] public bool viradoDireita = true;
	public bool jump;
	public BoxCollider2D bc;
	private bool sliding;
	float slidetime = 0f;
	public float maxslidetime = 0.1f;
	[SerializeField]
	GameObject Slidecollider;
	private bool attack = false; 
	private bool AirAttack = false;

	void Start () {

		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		bc = bc.GetComponent<BoxCollider2D> ();
		Slidecollider.gameObject.SetActive (false);
	}
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		//The groundcheck
		tocaChao = Physics2D.Linecast (transform.position, posPe.position, 1 << LayerMask.NameToLayer ("Ground"));
		if ((Input.GetKeyDown("space"))&& tocaChao) {
			jump = true;
				}
		if ((Input.GetKeyDown (KeyCode.J)) && tocaChao ) {
			attack = true;
		}
		if ((Input.GetKeyDown (KeyCode.J)) && !tocaChao ) {
			AirAttack = true;
		}
	}
	void FixedUpdate()
	{
		if (!isLocalPlayer) {
			return;
		} 
		//Player moviment
		float translationY = 0;
		float translationX = Input.GetAxis ("Horizontal") * Velocidade;
		transform.Translate (translationX, translationY, 0);
		transform.Rotate (0, 0, 0);
		//Animations
		if (translationX != 0 && tocaChao) {
			anim.SetTrigger ("Run");
			bc.size = new Vector3 (1.808332f, 3.115585f, 0);
			bc.offset = new Vector3 (0, 0, 0);
		} else {
			anim.SetTrigger ("Stand");
			bc.size = new Vector3 (1.240831f,3.081203f, 0);
			bc.offset = new Vector3 (0, 0, 0);
		}
		//Jump animation and force jump
		if (jump == true) {
			anim.SetTrigger ("Jump");
			bc.size = new Vector3 (1.245875f, 2.814565f, 0);
			bc.offset = new Vector3 (0.08144256f, 0.01039314f, 0);
			rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			sliding = false;
			jump = false;
			AirAttack = false;
		}
		//Animations after jump
		if (jump == false && translationX != 0 && tocaChao) {
			anim.SetTrigger ("Run");
		} else {
			anim.SetTrigger ("Stand");
		}		
		//Player direction
		if (translationX > 0 && !viradoDireita) {
			CmdFlip ();
		} else if (translationX < 0 && viradoDireita)
			CmdFlip ();
		//Slide script obs: need be fixed
		if ((Input.GetButton("Slide")) && tocaChao && !sliding  ) {
		slidetime = 0;
		anim.SetBool ("Slide", true);
		bc.enabled = false;
		sliding = true;
		Slidecollider.gameObject.SetActive (true);
	}
	if (sliding ) {
		slidetime += Time.deltaTime;
		if (slidetime > maxslidetime) {
			sliding = false;
			anim.SetBool ("Slide", false);
			bc.enabled = true;
			Slidecollider.gameObject.SetActive (false);
		}
	}
	if (!Input.GetButton ("Slide") && sliding) {
		anim.SetTrigger ("Stand");
		anim.SetBool ("Slide", false);
		sliding = false;
		Slidecollider.gameObject.SetActive (false);
		bc.enabled = true;
	        }
		//Regular attack
		if (attack == true) {
			anim.SetTrigger ("Attack");
			attack = false;
		}
		if (attack == false && translationX != 0 && tocaChao) {
			anim.SetTrigger ("Run");
		} else {
			anim.SetTrigger ("Stand");
		}		
		if (AirAttack == true) {
			anim.SetTrigger ("Air Attack");
			AirAttack = false;
		}
	}
	void  OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.CompareTag ("Obstaculo")) {
			anim.SetTrigger ("Hurt");
			jump = false;
			attack = false;
			sliding = false;
			anim.SetTrigger ("Stand");
			Update ();
		}
	}
	[Command]
	void CmdFlip(){
		RpcFlip ();
	}
	[ClientRpc]
	void RpcFlip(){
		viradoDireita = !viradoDireita;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
}
	
	


	


