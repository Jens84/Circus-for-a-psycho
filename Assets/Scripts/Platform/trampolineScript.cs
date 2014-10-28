using UnityEngine;
using System.Collections;

public class trampolineScript : MonoBehaviour {
	void Update () 
	{
		if (characterController.trampoline && !audio.isPlaying)
		{
			audio.Play();
			characterController.trampoline = false;
		}
	}
}