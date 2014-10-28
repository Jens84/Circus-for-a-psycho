using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {
	public static bool playerDied = false;			// used to reset the camera position
	public static float characterPositionY = 1.1f;	// used for vertical camera tracking 
	public static bool playerJumped = false;
	public static bool playerJumped2 = false;
	public static bool playerJumped3 = false;
	public static bool playerJumped4 = false;

	private bool facingRight = true;				// used to flip the player sprite
	
	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	// ======================================
	// GROUND CHECK
	// ======================================
	public LayerMask whatIsGround;					// defines which objects count as ground
	public Transform groundCheck;					// in-game physics test for ground and trampolin
	float groundRadius = 0.2f;						// used in conjunction with groundCheck
	bool grounded = false;							// is the player on the ground
	// ======================================
	// TRAMPOLINE CHECK
	// ======================================
	public LayerMask whatIsTrampoline;						// defines which objects count as trampolines
	bool trampoline = false;						// is the player on a trampoline
	
	Animator anim;
													// currently used for testing vertical bars script

	void Start () {
		anim = GetComponent<Animator> ();			// attaches animator to the player
	}
	
	void FixedUpdate () {
		// ======================================
		// CHECKS ARE MADE
		// ======================================
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		trampoline = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsTrampoline);
													// we should change this to a collision detection, so
													// trampoline can active/deactivate vertical platforms
													//currently we recieve several activations
		// ======================================
		// MOVING THE CHARACTER
		// ======================================
		float move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		// ======================================
		// ANIMATIONS VARIABLES
		// ======================================
		anim.SetBool ("Ground", grounded);					// for use in jumping
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);	// for use in jump and fall animation
		anim.SetFloat ("Speed", Mathf.Abs (move));			// for use in run animation
		// ======================================
		// CALLING SPRITE FLIPS
		// ======================================
		if (move > 0 && !facingRight) {			// flip player sprite, if walking opposite way of facing
			Flip ();
		}
		else if(move < 0 && facingRight){
			Flip ();
		}
		// ======================================
		// PLAYER FALLS FROM THE LEVEL
		// ======================================
		if (transform.position.y <= -4.0f) {
			transform.position = new Vector3(-6.0f,1.1f, -2.0f);
			playerDied = true;
		}
	}
	
	void Update () {
		characterPositionY = transform.position.y;			// used for vertical camera tracking
		// ======================================
		// JUMPING AND TRAMPOLINE
		// ======================================
		if (trampoline) {									// player will trampoline if on top of trampoline
			anim.SetBool ("Ground", false);
			rigidbody2D.velocity = new Vector2(0f,20f);
			trampoline = false;
			//playerJumped = true;							// activate to use in vertical platforms
			//playerJumped2 = true;							// activate to use in vertical platforms
		}
		else if (grounded && Input.GetButtonDown("Jump")) {	// player can jump if grounded
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
			playerJumped = true;							// used for vertical platforms
			playerJumped2 = true;							// used for vertical platforms
			playerJumped3 = true;							// used for horizontal platforms
			playerJumped4 = true;							// used for horizontal platforms
		}
		// ======================================
		// CAMERA CATCHES UP TO PLAYER
		// ======================================
		if (transform.position.x < (cameraControl.cameraPositionX-13)){ // kills player if he lags behind
			transform.position = new Vector3(-6.0f,1.1f, -2.0f);
			rigidbody2D.velocity = Vector2.zero;
			playerDied = true;
		}
		// ======================================
		// VERTICAL BAR FLIP
		// ======================================
	}
	// ======================================
	// PLAYER SPRITE FLIPS
	// ======================================

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}