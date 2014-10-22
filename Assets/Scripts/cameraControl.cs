using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
	public float speedHorizontal;				// horizontal camera speed
	public float speedVertical;					// vertical camera speed
	public static float cameraPositionX;		// send x-coordinate of screen to check if player lags behind
												// this way we only kill player on one side of the screen

	void FixedUpdate(){
		Vector3 targetPosition = new Vector3 (transform.position.x,
		                                    (characterController.characterPositionY + 0.5f), transform.position.z);

		transform.Translate(speedHorizontal * Time.deltaTime, 0, 0);

		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * speedVertical);
	}

	void Update(){
		cameraPositionX = transform.position.x;

		if (characterController.playerDied) {
			transform.position = new Vector3 (-5.5f, 3.6f, -8.0f);
			characterController.playerDied = false;
		}
	}
}
