using UnityEngine;
using System.Collections;

public class screenKill : MonoBehaviour {
	public static bool deathByScreen = false;

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
				deathByScreen = true;
		}
	}
}
