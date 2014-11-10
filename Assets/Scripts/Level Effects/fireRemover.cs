using UnityEngine;
using System.Collections;

public class fireRemover : MonoBehaviour {

	void Awake () {
		Destroy (gameObject, 10.0f);
	}
}
