using System;
using UnityEngine;

public class ChangeToLevel3 : MonoBehaviour
{	
	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Player")
			Application.LoadLevel(3);
	}			
}