using UnityEngine;
using System.Collections;

public class trampolineScript : MonoBehaviour {
	void OnCollisionEnter2D (Collision2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			audio.Play();
		}
	}
}