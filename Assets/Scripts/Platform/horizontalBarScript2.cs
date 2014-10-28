using UnityEngine;
using System.Collections;

public class horizontalBarScript2 : MonoBehaviour {
	bool activated = false;
	GameObject horizontalBar4;
	GameObject horizontalBar5;
	GameObject horizontalBar6;
	
	void Start(){
		horizontalBar4 = GameObject.FindWithTag("horizontalBar4");
		horizontalBar5 = GameObject.FindWithTag("horizontalBar5");
		horizontalBar6 = GameObject.FindWithTag("horizontalBar6");
	}
	
	void Update () {
		if (activated && characterController.playerJumped4) {
			characterController.playerJumped4 = false;
			horizontalBar4.SetActive (false);
			horizontalBar5.SetActive (false);
			horizontalBar6.SetActive (false);
			activated = !activated;
		}
		if (!activated && characterController.playerJumped4) {
			characterController.playerJumped4 = false;
			horizontalBar4.SetActive (true);
			horizontalBar5.SetActive (true);
			horizontalBar6.SetActive (true);
			activated = !activated;
		}
	}
}