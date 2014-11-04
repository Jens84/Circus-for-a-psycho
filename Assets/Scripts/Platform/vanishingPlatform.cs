using UnityEngine;
using System.Collections;

public class vanishingPlatform : MonoBehaviour {

	public float vanishTime;
	public float blinkingSpeed;
	public Color color;
	private bool isBlinking;
	
	void Start () {
		isBlinking = false;
		renderer.material.SetColor ("_Color", color);
		GameEventManager.Restart += Restart;
	}

	private void Restart () {
		isBlinking = false;
		gameObject.SetActive (true);
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Player" && !isBlinking) {
			StartCoroutine("Blinking");
			StartCoroutine("Destroying");
		}
	}

	IEnumerator Blinking() {
		isBlinking = true;
		Color c = color;
		float increment = -0.25F;

		while (true) {
			if(c.a == 0 || c.a == 1)
				increment = -increment; 

			c.a -= increment;
			renderer.material.SetColor ("_Color", c);
			yield return new WaitForSeconds (blinkingSpeed);
		}
	}

	IEnumerator Destroying() {
		yield return new WaitForSeconds(vanishTime);
		gameObject.SetActive(false);
	}
}
