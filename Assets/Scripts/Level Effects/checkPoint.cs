using UnityEngine;
using System.Collections;

public class checkPoint : MonoBehaviour {

	public static bool checkPointReached = false;
	public GameObject checkPointVFX;

	void OnTriggerEnter2D(Collider2D other){
		characterController.startPosition = transform.position;
		checkPointReached = true;
		Instantiate (checkPointVFX, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
		Destroy (gameObject, 2.0f);
	}
}