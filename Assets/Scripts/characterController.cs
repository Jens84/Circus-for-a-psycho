using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {
	// used to flip the character sprite
	private bool facingRight = true;

	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public float trampolinForce = 1000f;

	Animator anim;

	// ground check for use in jumping
	bool grounded = false;
	bool trampolin = false;
	bool inScreen = true;
	public Transform groundCheck;
	public Transform character;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public LayerMask whatIsTrampolin;
	public LayerMask whatIsScreenBorder;




	//bool doubleJumpUsed = false;

	// set animator to gameObject
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		// check if character is grounded
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		trampolin = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsTrampolin);
		inScreen = Physics2D.OverlapCircle (character.position, groundRadius, whatIsScreenBorder);
		anim.SetBool ("Ground", grounded);

		//if (grounded) {
		//	doubleJumpUsed = false;
		//}
		// update vSpeed boolean for use in animation
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		float move = Input.GetAxis ("Horizontal");

		anim.SetFloat ("Speed", Mathf.Abs (move));

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		if (move > 0 && !facingRight) {
			Flip ();
		}
		else if(move < 0 && facingRight){
			Flip ();
		}
		// ======================================
		// PLAYER RESTART POSITION
		// ======================================
		// player falls from the level
		if (transform.position.y <= -4.0f) {
			transform.position = new Vector2(-5.5f,1.1f);
		}


	}

	void Update () {
		// character can trampolin if it's grounded and on trampolin and player hits jump
		if (/*grounded && */trampolin/* && Input.GetButtonDown("Jump")*/) {
			anim.SetBool ("Ground", false);
			rigidbody2D.velocity = new Vector2(0f,20f);
			// rigidbody2D.AddForce(new Vector2(0, trampolinForce));
			trampolin = false;
		}
		// character can jump if it's grounded and player hits jump
		else if (grounded && Input.GetButtonDown("Jump")) { // ((grounded || !doubleJumpUsed) &&...
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));

		}
		// killerScreen
		if (!inScreen) {
			transform.position = new Vector2(-5.5f,1.1f);
			rigidbody2D.velocity = Vector2.zero;
		}
	}

	// flips the sprite from left to right, when changing direction in game
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}