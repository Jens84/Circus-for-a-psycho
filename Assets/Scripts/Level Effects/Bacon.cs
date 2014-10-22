using UnityEngine;
using System.Collections;

public class Bacon : MonoBehaviour {
	
	public Vector3 offset = new Vector3(0.0f, 2.5f, -2.0f);
	public Vector3 rotationVelocity = new Vector3(45, 100, 1);
	public float recycleOffset = 20.0f;
	public float spawnChance = 25.0f;

	void Start () {
		GameEventManager.GameOver += GameOver;
	}
	
	private void GameOver () {
		gameObject.SetActive(false);
	}
	
	void Update () {
		transform.Rotate(rotationVelocity * Time.deltaTime);
	}
	
	void OnTriggerEnter2D(Collider2D other){
		characterController.AddBacon();
		gameObject.SetActive(false);
	}
}