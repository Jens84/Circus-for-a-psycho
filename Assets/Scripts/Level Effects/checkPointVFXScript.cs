using UnityEngine;
using System.Collections;

public class checkPointVFXScript : MonoBehaviour {

	void Awake () {
		Destroy (gameObject, 3.0f);
	}
}