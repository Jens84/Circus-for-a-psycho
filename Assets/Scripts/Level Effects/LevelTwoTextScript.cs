using UnityEngine;
using System.Collections;

public class LevelTwoTextScript : MonoBehaviour {
	
	public AudioClip FreeTheAnimalsSound;
	
	void Awake () {
		audio.PlayOneShot(FreeTheAnimalsSound);
		Destroy (gameObject, 2.0f);
	}
}