using UnityEngine;
using System.Collections;

public class rotatorCheckPointScript : MonoBehaviour {
	Vector3 rotationVelocity = new Vector3(45, 0, 0);

	void FixedUpdate () {
		if (checkPoint.checkPointReached) {
			rotationVelocity = new Vector3(135, 0, 0);
		}
		transform.Rotate (rotationVelocity * Time.deltaTime);
	}
}