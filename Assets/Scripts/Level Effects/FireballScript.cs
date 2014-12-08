using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {
	
	void FixedUpdate () {
		transform.Translate(0, -5 * Time.deltaTime, 0);

		if (transform.position.y <198)
			Destroy(gameObject);
	}
}