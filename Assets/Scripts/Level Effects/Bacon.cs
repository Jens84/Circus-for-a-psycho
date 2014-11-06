using UnityEngine;
using System.Collections;

public class Bacon : MonoBehaviour {
	
	//public Vector3 offset = new Vector3(0.0f, 2.5f, -2.0f);
	public Vector3 rotationVelocity = new Vector3(45, 100, 1);
	//public float recycleOffset = 20.0f;
	//public float spawnChance = 25.0f;
	public GameObject baconCollected;

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

	void Update () {
		transform.Rotate(rotationVelocity * Time.deltaTime);
	}
	
	void OnTriggerEnter2D(Collider2D other){
		Instantiate (baconCollected, transform.position, transform.rotation);

		characterController.AddBacon();
		gameObject.SetActive (false);
	}

	void OnDestroy() {
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
	}
}