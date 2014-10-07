using UnityEngine;
using System.Collections;

public class PlatformManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			print("TriggerEnter");
			other.transform.parent = transform;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			print("TriggerEnter");
			other.transform.parent = null;
		}
	}
}
