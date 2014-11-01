using UnityEngine;
using System.Collections;

public class trampolineAnimationScript : MonoBehaviour {

	private Animator anim;
	
	void Start () {
		anim = GetComponent<Animator> ();	
	}

	void FixedUpdate(){
		anim.SetBool ("trampolineActivated", characterController.trampoline);
	}
}
