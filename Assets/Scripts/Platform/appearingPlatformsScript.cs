using UnityEngine;
using System.Collections;

public class appearingPlatformScript : MonoBehaviour {
	bool activatedA = true;
	bool activatedB = true;
	GameObject verticalBar1;
	GameObject verticalBar2;
	GameObject verticalBar3;
	GameObject verticalBar4;
	GameObject verticalBar5;
	GameObject verticalBar6;
	GameObject horizontalBar1;
	GameObject horizontalBar2;
	GameObject horizontalBar3;
	GameObject horizontalBar4;
	GameObject horizontalBar5;
	GameObject horizontalBar6;
	
	void Start(){
		verticalBar1 = GameObject.FindWithTag("verticalBar1");
		verticalBar2 = GameObject.FindWithTag("verticalBar2");
		verticalBar3 = GameObject.FindWithTag("verticalBar3");
		verticalBar4 = GameObject.FindWithTag("verticalBar4");
		verticalBar5 = GameObject.FindWithTag("verticalBar5");
		verticalBar6 = GameObject.FindWithTag("verticalBar6");
		horizontalBar1 = GameObject.FindWithTag("horizontalBar1");
		horizontalBar2 = GameObject.FindWithTag("horizontalBar2");
		horizontalBar3 = GameObject.FindWithTag("horizontalBar3");
		horizontalBar4 = GameObject.FindWithTag("horizontalBar4");
		horizontalBar5 = GameObject.FindWithTag("horizontalBar5");
		horizontalBar6 = GameObject.FindWithTag("horizontalBar6");

	}
	
	void Update () {
		if (activatedA && characterController.playerJumped) {
			verticalBar1.SetActive (false);
			verticalBar2.SetActive (false);
			verticalBar3.SetActive (false);
			horizontalBar1.SetActive (false);
			horizontalBar2.SetActive (false);
			horizontalBar3.SetActive (false);
			activatedA = !activatedA;
		}
		if (!activatedA && characterController.playerJumped) {
			verticalBar1.SetActive (true);
			verticalBar2.SetActive (true);
			verticalBar3.SetActive (true);
			horizontalBar1.SetActive (true);
			horizontalBar2.SetActive (true);
			horizontalBar3.SetActive (true);
			activatedA = !activatedA;
		}
		if (activatedB && characterController.playerJumped) {
			verticalBar4.SetActive (false);
			verticalBar5.SetActive (false);
			verticalBar6.SetActive (false);
			horizontalBar4.SetActive (false);
			horizontalBar5.SetActive (false);
			horizontalBar6.SetActive (false);
			activatedB = !activatedB;
		}
		if (!activatedB && characterController.playerJumped) {
			verticalBar4.SetActive (true);
			verticalBar5.SetActive (true);
			verticalBar6.SetActive (true);
			horizontalBar4.SetActive (true);
			horizontalBar5.SetActive (true);
			horizontalBar6.SetActive (true);
			activatedB = !activatedB;
		}
		characterController.playerJumped = false;
	}
}