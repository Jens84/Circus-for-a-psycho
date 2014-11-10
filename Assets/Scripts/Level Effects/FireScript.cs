using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour 
{

	public Rigidbody2D fire;				// Prefab of the fire.
	public float fireSpeed = 20.0f;

	private LionScript lionCtrl;			// Reference to the LionScript

	void OnCollisionEnter2D(Collision2D c) 
	{

	}

	void Awake()
	{
		// Setting up the references.
		lionCtrl = transform.root.GetComponent<LionScript>();
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (LionScript.aqcuiredFire) 
		{
			if (lionCtrl.facingRight) {
				// ... instantiate the fire facing right and set it's velocity to the right. 
				Rigidbody2D fireInstance = Instantiate (fire, transform.position, Quaternion.Euler (new Vector3 (0, 0, 90f))) as Rigidbody2D;
				fireInstance.velocity = new Vector2 (fireSpeed, 0);
			} else {
				// Otherwise instantiate the fire facing left and set it's velocity to the left.
				Rigidbody2D fireInstance = Instantiate (fire, transform.position, Quaternion.Euler (new Vector3 (0, 0, 270f))) as Rigidbody2D;
				fireInstance.velocity = new Vector2 (-fireSpeed, 0);
			}
		}
	}
}
