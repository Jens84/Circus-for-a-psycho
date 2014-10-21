using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
	public float speed;

	void FixedUpdate(){
		transform.Translate(speed * Time.deltaTime, 0, 0);
	}

	void Update(){
		if (characterController.playerDied) {
			transform.position = new Vector3 (-5.5f, 3.6f, -8.0f);
			characterController.playerDied = false;
		}
	}
}
