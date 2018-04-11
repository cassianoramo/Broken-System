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

	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		bc = bc.GetComponent<BoxCollider2D> ();
	}
	void Update () {
		//Verifica se o player está tocando o chão
		tocaChao = Physics2D.Linecast (transform.position, posPe.position, 1 << LayerMask.NameToLayer ("Ground"));
		if ((Input.GetKeyDown("space"))&& tocaChao) {
			jump = true;
				}
	}
	void FixedUpdate()
	{
		//Movimentação do personagem
		float translationY = 0;
		float translationX = Input.GetAxis ("Horizontal") * Velocidade;
		transform.Translate (translationX, translationY, 0);
		transform.Rotate (0, 0, 0);
		//Define as animações que serão feitas
		if (translationX != 0 && tocaChao) {
			anim.SetTrigger ("Run");
			bc.size = new Vector3 (1.808332f, 3.115585f, 0);
			bc.offset = new Vector3 (0, -0.25f, 0);
		} else {
			anim.SetTrigger ("Stand");
			bc.size = new Vector3 (1.240831f, 3.5f, 0);
			bc.offset = new Vector3 (0, 0, 0);
		}
		//Chama a animação do pulo e define sua força
		if (jump == true) {
			anim.SetTrigger ("Jump");
			bc.size = new Vector3 (1.245875f, 2.814565f, 0);
			bc.offset = new Vector3 (0.08144256f, 0.01039314f, 0);
			rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			jump = false;
		}
		//Chama as animações pós-pulo
		if (jump == false && translationX != 0 && tocaChao) {
			anim.SetTrigger ("Run");
		} else {
			anim.SetTrigger ("Stand");
		}			
		//Define a direção ao qual o player está posicionado
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
	//Faz o personagem andar para as duas direções
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
	


