using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {
	
	public GameObject balloonBurst;

	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
	}

	private void GameStart () {
		gameObject.SetActive (true);
	}

	private void GameOver () {
		gameObject.SetActive(false);
	}

	void FixedUpdate () {
		transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Knife"){
			Instantiate (balloonBurst, transform.position, transform.rotation);
			characterController.AddBalloon ();
			Destroy (gameObject);
		}
	}

	void OnDestroy() {
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
	}
}