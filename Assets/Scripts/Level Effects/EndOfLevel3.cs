using UnityEngine;

public class EndOfLevel3 : MonoBehaviour {

	public static bool elevate = false;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Player>() == null)
			return;

		elevate = true;
	}

	public void FixedUpdate()
	{
		if (transform.position.y > 210 && elevate == true)
			transform.Translate (0, -5 * Time.deltaTime, 0);
	}
}
