using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public float movingLength;
	public float speed;

	private float startPosition;
	private int currentDirection;

	// Use this for initialization
	void Start () {
		currentDirection = -1;
		startPosition = transform.position.x;
	}

	void Update () {
		
	}

	void FixedUpdate () {
		if (transform.position.x <= (startPosition - movingLength/2)
		    || transform.position.x >= (startPosition + movingLength/2))
			currentDirection *= -1;
		
		transform.Translate (currentDirection * speed/100, 0, 0);
	}
}
