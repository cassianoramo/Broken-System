using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
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
	private float slidetime = 0f;
	public float maxslidetime = 0.5f;
	[SerializeField]
	GameObject Slidecollider;

	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		bc = bc.GetComponent<BoxCollider2D> ();
		Slidecollider.gameObject.SetActive (false);
	}
	void Update () {
		//The groundcheck
		tocaChao = Physics2D.Linecast (transform.position, posPe.position, 1 << LayerMask.NameToLayer ("Ground"));
		if ((Input.GetKeyDown("space"))&& tocaChao) {
			jump = true;
				}
		//Slide condition
		if ((Input.GetButton("Slide")) && tocaChao && !sliding ) {
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
	}
	void FixedUpdate()
	{
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
		}
		//Animatons after jump
		if (jump == false && translationX != 0 && tocaChao) {
			anim.SetTrigger ("Run");
		} else {
			anim.SetTrigger ("Stand");
		}		
		//Player direction
		if (translationX > 0 && !viradoDireita) {
			Flip (); 
		} else if (translationX < 0 && viradoDireita)
			Flip (); 
		if (translationX > 0 && !viradoDireita) {
			Flip ();
		} else if (translationX < 0 && viradoDireita) {
			Flip ();
		  }
	}
	//Flip script
	void Flip()
	{
		viradoDireita = !viradoDireita;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
	//Método de dano do player
	/*public void SubtraiVida()
	{
		vida.fillAmount-=0.1f;
		if (vida.fillAmount <= 0) {
			MC.GameOver();
			Destroy(gameObject);
		}
	}*/
		}
	


