using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {
	public static bool playerDied = false;
	public static float characterPositionY = 1.1f;

	private bool facingRight = true;		// used to flip the character sprite

	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public float trampolineForce = 1000f;

	Animator anim;							// attaches animations to the character
	
	bool grounded = false;					// ground check for use in jumping
	public static bool trampoline = false;	// check for use in jumping on trampoline
	bool inScreen = true;					// is the character outside of camera bounds
	public Transform groundCheck;			// in-game physics test for ground and trampoline
	public Transform character;				// in-game physics test for camera bounds
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;			// which gameObject counts as ground
	public LayerMask whatIsTrampoline;		// which gameObject counts as trampoline
	public LayerMask whatIsScreenBorder;	// bounding box of the camera - CURRENTLY NOT USED




	//bool doubleJumpUsed = false;

	// set animator to gameObject
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		// check if character is grounded
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		// check if character is on top of trampoline
		trampoline = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsTrampoline);
		// check if character is on the screen - CURRENTLY NOT USED
		inScreen = Physics2D.OverlapCircle (character.position, groundRadius, whatIsScreenBorder);

		//if (grounded) {
		//	doubleJumpUsed = false;
		//}

		float move = Input.GetAxis ("Horizontal");

		// ======================================
		// UPDATING ANIMATION VARIABLES
		// ======================================
		// update Ground boolean for use in animation
		anim.SetBool ("Ground", grounded);
		// update vSpeed boolean for use in jump animation
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		// update Speed boolean for use in run animation
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
		// Later we should make a CHECKPOINT RESTART Vector3 for each checkpoint
		// --------------------------------------
		// player falls from the level
		if (transform.position.y <= -4.0f) {
			transform.position = new Vector3(-6.0f,1.1f, -2.0f);
			playerDied = true;
		}
	}

	void Update () {
		characterPositionY = transform.position.y;
		// character will trampoline if on top of trampoline
		if (trampoline) {
			anim.SetBool ("Ground", false);
			rigidbody2D.velocity = new Vector2(0f,20f);
		}
		// character can jump if it's grounded and player hits jump
		else if (grounded && Input.GetButtonDown("Jump")) { // ((grounded || !doubleJumpUsed) &&...
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}
		// ======================================
		// CHECK IF SCREEN CATCHES UP WITH PLAYER
		// ======================================
		if (!inScreen) {
			if (transform.position.x < (cameraControl.cameraPositionX-13)){ // kills player if he lags behind
				transform.position = new Vector3(-6.0f,1.1f, -2.0f);
				rigidbody2D.velocity = Vector2.zero;
				playerDied = true;
			}
		// ======================================
		// THIS FEATURE IS ACTUALLY REALLY COOL
		// ======================================
		// PLAYER EXITING SCREEN ON RIGHT ENTERS SCREEN ON LEFT
		// --------------------------------------
		//if (transform.position.x > (cameraControl.cameraPositionX+11)){
		//	transform.position = new Vector3(cameraControl.cameraPositionX-11,
		//	                                 transform.position.y, transform.position.z);
		//}
		}
	}

	// ======================================
	// FLIP SPRITE ACCORDING TO PLAYER DIRECTION
	// ======================================
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}