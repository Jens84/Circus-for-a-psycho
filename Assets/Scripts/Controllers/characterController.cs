﻿using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {

	Animator anim;
	float groundRadius = 0.2f;						// used in conjunction with groundCheck
	bool grounded = false;							// is the player on the ground
	bool trampoline = false;						// is the player on a trampoline

	public static bool playerDied = false;			// used to reset the camera position
	public static float characterPositionY = 1.1f;	// used for vertical camera tracking 
	public static bool playerJumped = false;
	public static bool playerJumped2 = false;
	public static bool playerJumped3 = false;
	public static bool playerJumped4 = false;
	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public LayerMask whatIsGround;					// defines which objects count as ground
	public LayerMask whatIsTrampoline;				// defines which objects count as trampolines
	public Transform groundCheck;
	public float gameOverY = -4.0f;

	private static int bacon;
	private Vector3 startPosition;
	private bool facingRight = true;				// used to flip the player sprite

	void Start () {
		anim = GetComponent<Animator> ();			// attaches animator to the player
		// Connect to the GameEventManager
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		startPosition = new Vector3(-6.0f,1.1f, -2.0f);
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
	
	public static void AddBacon () {
		bacon += 1;
		GUIManager.SetBacon(bacon);
	}
	
	private void GameOver () {
		//Application.LoadLevel (0);
		renderer.enabled = false;
		rigidbody2D.isKinematic = true;
		enabled = false;
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
		if (transform.position.y <= gameOverY) {
			transform.position = startPosition;
			playerDied = true;
			GameEventManager.TriggerGameOver();
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
			transform.position = startPosition;
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