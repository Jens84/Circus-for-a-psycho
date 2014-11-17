using UnityEngine;
using System.Collections;

public class fallingLeavesPlatform : MonoBehaviour {
	public float speed;

	public Transform platform;
	public Transform startTransform;
	public Transform endTransform;

	void FixedUpdate() {
		Vector3 targetPosition = new Vector3 (endTransform.position.x, endTransform.position.y,
		                                      transform.position.z);
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * speed);

		if (Vector3.Distance (transform.position, endTransform.position) < 3.0f) {
			transform.position = startTransform.position; 
		}
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube (startTransform.position, platform.localScale);
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (endTransform.position, platform.localScale);
	}
}
