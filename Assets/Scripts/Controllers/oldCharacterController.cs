using UnityEngine;
using System.Collections;
public class oldCharacterController : MonoBehaviour {

	public static bool playerDied = false;
	private bool facingRight = true;	// used to flip the character sprite
	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public float trampolineForce = 1000f;
	Animator anim;	// attaches animations to the character
	public bool grounded = false;	// ground check for use in jumping
	bool movingOnPlatform = false;
	MovingPlatform currentMovingPlatform;

	public static bool trampoline = false;	// check for use in jumping on trampolin
	bool inScreen = true;	// is the character outside of camera bounds
	public Transform groundCheck;	// in-game physics test for ground and trampolin
	public Transform character;	// in-game physics test for camera bounds
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public LayerMask whatIsTrampoline;
	public LayerMask whatIsScreenBorder;
	//bool doubleJumpUsed = false;

	// set animator to gameObject
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "MovingPlatform" && grounded) {
			movingOnPlatform = true;
			currentMovingPlatform = c.gameObject.transform.parent.GetComponent<MovingPlatform>();
		}
	}
	
	void OnCollisionExit2D(Collision2D c) {
		if (c.gameObject.tag == "MovingPlatform") {
			movingOnPlatform = false;
			currentMovingPlatform = null;
		}
	}

	void FixedUpdate () {
		// check if character is grounded
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		trampoline = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsTrampoline);
		inScreen = Physics2D.OverlapCircle (character.position, groundRadius, whatIsScreenBorder);
		anim.SetBool ("Ground", grounded);
		//if (grounded) {
		// doubleJumpUsed = false;
		//}
		// update vSpeed boolean for use in animation

		// ======================================
		// PLAYER RESTART POSITION (CHECKPOINT)
		// ======================================
		// player falls from the level
		if (transform.position.y <= -4.0f) {
			transform.parent = null;
			transform.position = new Vector3(-5.5f,1.1f, -2.0f);
			playerDied = true;
		}
	}

	void Update () {
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (move));

		if(movingOnPlatform)
			rigidbody2D.MovePosition (transform.position
			                          + currentMovingPlatform.direction * 1.35f
			                          * currentMovingPlatform.platformSpeed 
			                          * Time.fixedDeltaTime
			                          + new Vector3 (move * maxSpeed, rigidbody2D.velocity.y, 0)
			                          * Time.fixedDeltaTime);
		else
			rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		if (move > 0 && !facingRight) {
			Flip ();
		}
		else if(move < 0 && facingRight){
			Flip ();
		}
		// character will trampoline if on top of trampoline
		if (trampoline) {
			anim.SetBool ("Ground", false);
			rigidbody2D.velocity = new Vector2(0f,20f);
		}
		// character can jump if it's grounded and player hits jump
		else if (grounded && Input.GetButtonDown("Jump")) { // ((grounded || !doubleJumpUsed) &&...
			anim.SetBool ("Ground", false);
			movingOnPlatform = false;
			currentMovingPlatform = null;
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}
		// ======================================
		// CHECK IF PLAYER IS OUTSIDE SCREEN
		// ======================================
		if (!inScreen) {
			if (transform.position.x < (cameraControl.cameraPositionX-13)){ // kills player if he lags behind
				transform.parent = null;
				transform.position = new Vector3(-5.5f,1.1f, -2.0f);
				rigidbody2D.velocity = Vector2.zero;
				playerDied = true;
			}
			//if (transform.position.x > (cameraControl.cameraPositionX+11)){
			// transform.position = new Vector3(cameraControl.cameraPositionX-11,
			// transform.position.y, transform.position.z);
			//}
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