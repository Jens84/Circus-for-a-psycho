using UnityEngine;
using System.Collections;

public class StopElevatorSparkles : MonoBehaviour {

	public void FixedUpdate()
	{		
		if (EndOfLevel3.elevate == true)
			Destroy(gameObject, 1.5f);
	}
}