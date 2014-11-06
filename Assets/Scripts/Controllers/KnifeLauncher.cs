using UnityEngine;
using System.Collections;

public class KnifeLauncher : MonoBehaviour
{
	public Rigidbody2D knife;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
	Animator anim;
	
	private characterController playerCtrl;		// Reference to the PlayerControl script.
	
	
	void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<characterController>();
	}
	
	
	void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButtonDown("Action"))
		{
			// ... set the animator Shoot trigger parameter and play the audioclip.
			//anim.SetTrigger("Shoot");
			//audio.Play();
			
			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D knifeInstance = Instantiate(knife, transform.position, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
				knifeInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D knifeInstance = Instantiate(knife, transform.position, Quaternion.Euler(new Vector3(0,0,270f))) as Rigidbody2D;
				knifeInstance.velocity = new Vector2(-speed, 0);
			}
		}
	}
}
