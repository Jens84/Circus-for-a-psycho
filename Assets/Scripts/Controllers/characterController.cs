using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {	

	Animator anim;
	bool grounded = false;							// is the player on the ground
	bool movingOnPlatform = false;					// is the player on a moving platform
	float groundRadius = 0.2f;						// used in conjunction with groundCheck

	public static bool trampoline = false;			// is the player on a trampoline
	public static bool playerDied = false;			// used to reset the camera position	 
	public static bool playerJumped = false;
	public static float characterPositionY = 1.1f;	// used for vertical camera tracking
	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public float gameOverY = -13.0f;
	public LayerMask whatIsGround;					// defines which objects count as ground
	public LayerMask whatIsTrampoline;				// defines which objects count as trampolines
	public Transform groundCheck;

	private static int bacon;
	private bool facingRight = true;				// used to flip the player sprite
	private float move;
	private MovingPlatform currentMovingPlatform;	// the moving platform the player is on
	private Vector3 startPosition;


	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "MovingPlatform" && grounded) {
			movingOnPlatform = true;
			currentMovingPlatform = c.gameObject.transform.parent.GetComponent<MovingPlatform>();
		}
		// ======================================
		// PLAYER HITS SPIKES
		// ======================================
		if (c.gameObject.tag == "Spikes")
		{
			transform.position = startPosition;
			playerDied = true;
			GameEventManager.TriggerGameOver ();
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (c.gameObject.tag == "MovingPlatform") {
			movingOnPlatform = false;
			currentMovingPlatform = null;
		}
	}

	void Start () {
		anim = GetComponent<Animator> ();			// attaches animator to the player
		// Connect to the GameEventManager
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.Restart += Restart;
		startPosition = new Vector3(-6.0f, 1.1f, -2.0f);
		renderer.enabled = false;					// Disable the character before game starts
		rigidbody2D.isKinematic = true;				// Used for GameStart and GameOver
		enabled = false;
	}

	private void GameStart () {
		bacon = 0;
		GUIManager.SetBacon(bacon);
		transform.localPosition = startPosition;
		renderer.enabled = true;
		rigidbody2D.isKinematic = false;
		enabled = true;
	}

	private void Restart () {
		GameEventManager.TriggerGameOver();
		GameEventManager.TriggerGameStart();
	}
	
	private void GameOver () {
		//Application.LoadLevel (0);
		renderer.enabled = false;
		rigidbody2D.isKinematic = true;
		enabled = false;
	}
	
	public static void AddBacon () {
		bacon += 1;
		GUIManager.SetBacon(bacon);
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
		// ANIMATIONS VARIABLES
		// ======================================
		anim.SetBool ("Ground", grounded);					// for use in jumping
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);	// for use in jump and fall animation
		anim.SetFloat ("Speed", Mathf.Abs (move));
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
		if (transform.position.y <= gameOverY) {
			transform.position = startPosition;
			playerDied = true;
			GameEventManager.TriggerRestart();
		}
	}
	
	void Update () {
		move = Input.GetAxis ("Horizontal");				// for use in run animation
		characterPositionY = transform.position.y;			// used for vertical camera tracking
		// ======================================
		// MOVING THE CHARACTER
		// ======================================
		if(movingOnPlatform)
			rigidbody2D.MovePosition (transform.position
			                          + currentMovingPlatform.direction * 1.35f
			                          * currentMovingPlatform.platformSpeed 
			                          * Time.fixedDeltaTime
			                          + new Vector3 (move * maxSpeed, rigidbody2D.velocity.y, 0)
			                          * Time.fixedDeltaTime);
		else
			rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		// ======================================
		// JUMPING AND TRAMPOLINE
		// ======================================
		if (trampoline) {									// player will trampoline if on top of trampoline
			anim.SetBool ("Ground", false);
		}
		else if (grounded && Input.GetButtonDown("Jump") && !trampoline) {	// player can jump if grounded
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
			playerJumped = true;							// used for vertical platforms
		}
		// ======================================
		// CAMERA CATCHES UP TO PLAYER
		// ======================================
		if (transform.position.x < (cameraControl.cameraPositionX-11.5)){ // kills player if he lags behind
			rigidbody2D.velocity = Vector2.zero;
			transform.position = startPosition;
			playerDied = true;
			GameEventManager.TriggerRestart();
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

	void OnDestroy() {
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
	}
}