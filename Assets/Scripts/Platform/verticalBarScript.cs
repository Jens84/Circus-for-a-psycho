using UnityEngine;
using System.Collections;

public class verticalBarScript : MonoBehaviour {
	bool activated = true;
	GameObject verticalBar1;
	GameObject verticalBar2;
	GameObject verticalBar3;
	GameObject horizontalBar1;
	GameObject horizontalBar2;
	GameObject horizontalBar3;
	
	void Start(){
		verticalBar1 = GameObject.FindWithTag("verticalBar1");
		verticalBar2 = GameObject.FindWithTag("verticalBar2");
		verticalBar3 = GameObject.FindWithTag("verticalBar3");
		horizontalBar1 = GameObject.FindWithTag("horizontalBar1");
		horizontalBar2 = GameObject.FindWithTag("horizontalBar2");
		horizontalBar3 = GameObject.FindWithTag("horizontalBar3");
	}
	
	void Update () {
		if (activated && characterController.playerJumped) {
			characterController.playerJumped = false;
			verticalBar1.SetActive (false);
			verticalBar2.SetActive (false);
			verticalBar3.SetActive (false);
			horizontalBar1.SetActive (false);
			horizontalBar2.SetActive (false);
			horizontalBar3.SetActive (false);
			activated = !activated;
		}
		if (!activated && characterController.playerJumped) {
			characterController.playerJumped = false;
			verticalBar1.SetActive (true);
			verticalBar2.SetActive (true);
			verticalBar3.SetActive (true);
			horizontalBar1.SetActive (true);
			horizontalBar2.SetActive (true);
			horizontalBar3.SetActive (true);
			activated = !activated;
		}
	}
}