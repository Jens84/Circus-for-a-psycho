using UnityEngine;
using System.Collections;

public class PlusChanger : MonoBehaviour
{
	bool activated = true;
	// set A of objects
	GameObject RedPlus;

	// set B of objects
	GameObject GreyPlus;
	
	void Start()
	{
		// set A of objects
		RedPlus = GameObject.FindWithTag("RedPlus");
		// set B of objects
		GreyPlus = GameObject.FindWithTag("GreyPlus");
	}
	
	void Update()
	{
		if (activated && CharacterController2D.PlayerJumped)
		{
			CharacterController2D.PlayerJumped = false;
			// set A of objects
			RedPlus.SetActive(false);
			// set B of objects
			GreyPlus.SetActive(true);
			
			activated = !activated;
		}
		if (!activated && CharacterController2D.PlayerJumped)
		{
			CharacterController2D.PlayerJumped = false;
			// set A of objects
			RedPlus.SetActive(true);
			// set B of objects
			GreyPlus.SetActive(false);
			
			activated = !activated;
		}
	}
}