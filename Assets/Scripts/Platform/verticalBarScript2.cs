using UnityEngine;
using System.Collections;

public class verticalBarScript2 : MonoBehaviour {
	bool activated = false;
	GameObject verticalBar4;
	GameObject verticalBar5;
	GameObject verticalBar6;
	
	void Start(){
		verticalBar4 = GameObject.FindWithTag("verticalBar4");
		verticalBar5 = GameObject.FindWithTag("verticalBar5");
		verticalBar6 = GameObject.FindWithTag("verticalBar6");
	}
	
	void Update () {
		if (activated && characterController.playerJumped2) {
			characterController.playerJumped2 = false;
			verticalBar4.SetActive (false);
			verticalBar5.SetActive (false);
			verticalBar6.SetActive (false);
			activated = !activated;
		}
		if (!activated && characterController.playerJumped2) {
			characterController.playerJumped2 = false;
			verticalBar4.SetActive (true);
			verticalBar5.SetActive (true);
			verticalBar6.SetActive (true);
			activated = !activated;
		}
	}
}