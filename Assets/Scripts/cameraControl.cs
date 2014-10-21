using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
	public float speed;
	public static float cameraPositionX;		// send x-coordinate of screen to check if player lags behind
												// this way we only kill player on one side of the screen

	void FixedUpdate(){
		transform.Translate(speed * Time.deltaTime, 0, 0);
	}

	void Update(){
		cameraPositionX = transform.position.x;

		if (characterController.playerDied) {
			transform.position = new Vector3 (-5.5f, 3.6f, -8.0f);
			characterController.playerDied = false;
		}
	}
}
