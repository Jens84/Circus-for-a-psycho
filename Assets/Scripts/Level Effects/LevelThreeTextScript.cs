using UnityEngine;
using System.Collections;

public class LevelThreeTextScript : MonoBehaviour {
	
	public AudioClip EscapeTheCircusSound;
	
	void Awake () {
		audio.PlayOneShot(EscapeTheCircusSound);
		Destroy (gameObject, 2.0f);
	}
}