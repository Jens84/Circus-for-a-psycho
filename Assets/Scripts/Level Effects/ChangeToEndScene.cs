using System;
using UnityEngine;

public class ChangeToEndScene : MonoBehaviour
{	
	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Player")
			Application.LoadLevel(4);
	}			
}