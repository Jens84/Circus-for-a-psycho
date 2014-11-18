using UnityEngine;
using System.Collections;

public class balloonBurst : MonoBehaviour {

	void Awake () {
		Destroy (gameObject, 0.2f);
	}
}