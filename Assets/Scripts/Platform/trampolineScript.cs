using UnityEngine;
using System.Collections;

public class trampolineScript : MonoBehaviour {

	void OnCollision2D (Collision2D other)
	{
		if ((other.gameObject.tag == "Player" || characterController.trampoline) && !audio.isPlaying)
		{
			audio.Play();
			other.gameObject.rigidbody2D.velocity = new Vector2(0f,20f);
		}
	}
}