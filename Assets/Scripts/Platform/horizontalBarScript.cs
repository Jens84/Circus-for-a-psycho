using UnityEngine;
using System.Collections;

public class horizontalBarScript : MonoBehaviour {
	bool activated = true;
	GameObject horizontalBar1;
	GameObject horizontalBar2;
	GameObject horizontalBar3;
	
	void Start(){
		horizontalBar1 = GameObject.FindWithTag("horizontalBar1");
		horizontalBar2 = GameObject.FindWithTag("horizontalBar2");
		horizontalBar3 = GameObject.FindWithTag("horizontalBar3");
	}
	
	void Update () {
		if (activated && characterController.playerJumped3) {
			characterController.playerJumped3 = false;
			horizontalBar1.SetActive (false);
			horizontalBar2.SetActive (false);
			horizontalBar3.SetActive (false);
			activated = !activated;
		}
		if (!activated && characterController.playerJumped3) {
			characterController.playerJumped3 = false;
			horizontalBar1.SetActive (true);
			horizontalBar2.SetActive (true);
			horizontalBar3.SetActive (true);
			activated = !activated;
		}
	}
}