using UnityEngine;
using System.Collections;

public class baconCollected : MonoBehaviour {

	void Awake () {
		Destroy (gameObject, 1.0f);
	}
}