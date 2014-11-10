using UnityEngine;
using System.Collections;

public class LionScript : MonoBehaviour {

	public Transform target;
	public float speed = 0.5f;
	public static bool aqcuiredFire = false;

	[HideInInspector]
	public bool facingRight = true;					// used to flip the lion sprite

	private bool reachedPickUp;
	
	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "FirePickUp") {
			c.gameObject.SetActive(false);
			reachedPickUp = true;
		}
	}
	
	void Start () {
		// Connect to the GameEventManager
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.Restart += Restart;
		renderer.enabled = false;					// Disable the lion before game starts
		rigidbody2D.isKinematic = true;				// Used for GameStart and GameOver
		enabled = false;
	}
	
	private void GameStart () {
		renderer.enabled = true;
		rigidbody2D.isKinematic = false;
		enabled = true;
	}
	
	private void Restart () {
		GameEventManager.TriggerGameOver();
		GameEventManager.TriggerGameStart();
	}
	
	private void GameOver () {
		renderer.enabled = false;
		rigidbody2D.isKinematic = true;
		enabled = false;
	}
	
	// Update is called once per frame
	void Update() {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		if (reachedPickUp) pickUpFire ();
	}
	
	void pickUpFire()
	{
		speed = 0;
		reachedPickUp = false;
		aqcuiredFire = true;
	}
	
	void OnDestroy() {
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
	}
}
