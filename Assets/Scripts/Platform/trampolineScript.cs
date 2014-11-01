using UnityEngine;
using System.Collections;

public class trampolineScript : MonoBehaviour {
	
	public AudioClip trampolineSound;

	void OnCollisionEnter2D (Collision2D other)
	{
		if ((other.gameObject.tag == "Player") && !audio.isPlaying)
		{
			audio.PlayOneShot(trampolineSound);
			other.gameObject.rigidbody2D.velocity = new Vector2(0f,20f);
			characterController.playerJumped = true;							// activate to use in vertical platforms
		}
	}

}