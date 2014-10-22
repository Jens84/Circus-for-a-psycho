using UnityEngine;
using System.Collections;

public class trampolineScript : MonoBehaviour {
	
	public AudioClip Trampoline;
	
	void Update () {
		if (characterController.trampoline && !audio.isPlaying){
			audio.clip = Trampoline;
			audio.Play ();
			characterController.trampoline = false;
		}
	}
}