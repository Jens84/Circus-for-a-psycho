using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {
	public static bool playerDied = false;
	private bool facingRight = true;		// used to flip the character sprite

	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public float trampolinForce = 1000f;

	Animator anim;							// attaches animations to the character
	
	bool grounded = false;					// ground check for use in jumping
	bool trampolin = false;					// check for use in jumping on trampolin
	bool inScreen = true;					// is the character outside of camera bounds
	public Transform groundCheck;			// in-game physics test for ground and trampolin
	public Transform character;				// in-game physics test for camera bounds
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
		// PLAYER RESTART POSITION (CHECKPOINT)
		// ======================================
		// player falls from the level
		if (transform.position.y <= -4.0f) {
			transform.position = new Vector3(-5.5f,1.1f, -2.0f);
			playerDied = true;
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
		// ======================================
		// CHECK IF PLAYER IS OUTSIDE SCREEN
		// ======================================
		if (!inScreen) {
			if (transform.position.x < (cameraControl.cameraPositionX-13)){ // kills player if he lags behind
				transform.position = new Vector3(-5.5f,1.1f, -2.0f);
				rigidbody2D.velocity = Vector2.zero;
				playerDied = true;
			}
			if (transform.position.x > (cameraControl.cameraPositionX+11)){
				transform.position = new Vector3(cameraControl.cameraPositionX-11,
				                                 transform.position.y, transform.position.z);
			}
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