using UnityEngine;
using System.Collections;

public class CheckPointVFXScript : MonoBehaviour {

	void Awake () {
		Destroy (gameObject, 3.0f);
	}
}