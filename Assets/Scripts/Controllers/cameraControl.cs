using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
	public float speedHorizontal;				// horizontal camera speed
	public float speedVertical;					// vertical camera speed
	public static float cameraPositionX;		// send x-coordinate of screen to check if player lags behind
												// this way we only kill player on left side of the screen

	void Start () {
		// Connect to the GameEventManager
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		enabled = false;
	}

	private void GameStart () {
		enabled = true;
	}
	
	private void GameOver () {
		enabled = false;
	}

	void FixedUpdate(){
		// ======================================
		// TRACKING PLAYER VERTICALLY
		// ======================================
		Vector3 targetPosition = new Vector3 (transform.position.x,
		                                      (characterController.characterPositionY - 0.5f), transform.position.z);
		// ======================================
		// HORIZONTAL CAMERA
		// ======================================
		transform.Translate(Time.deltaTime * speedHorizontal, 0, 0);
		// ======================================
		// VERTICAL CAMERA TRACKING
		// ======================================
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * speedVertical);
	}

	void Update(){
		cameraPositionX = transform.position.x;		// used to kill player that lags behind

		if (characterController.playerDied) {		// reset camera position upon player death
			transform.position = new Vector3 (-6.0f, 1.6f, -8.0f);
			characterController.playerDied = false;
		}
	}
}