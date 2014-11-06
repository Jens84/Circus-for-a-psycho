using UnityEngine;
using System.Collections;

public class knifeRemover : MonoBehaviour {
	
	void Awake () {
		Destroy (gameObject, 2.0f);
	}
}