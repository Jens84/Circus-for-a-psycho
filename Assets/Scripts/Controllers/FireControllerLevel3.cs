using UnityEngine;
using System.Collections;

public class FireControllerLevel3 : MonoBehaviour, IPlayerRespawnListener
{
	public float FireSpeed = 5;

	private Player _player;
	private Vector3 _startPosition;

	public void Awake()
	{
		_player = GameObject.FindObjectOfType<Player>();
		_startPosition = transform.position;
	}

	public void FixedUpdate () {
		if (_player.IsDead) {
			transform.position = _startPosition;
			//gameObject.SetActive(false);
			return;
		}

		if (transform.position.y <= 205)
		{
			if (transform.position.y > 100)
				transform.Translate(0, 0.6f * FireSpeed * Time.deltaTime, 0);
			else
				transform.Translate(0, FireSpeed * Time.deltaTime, 0);
		}
	}

	// From IPlayerRespawnListener, to respond when the player get reInstantiated
	public void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player)
	{
		transform.localScale = new Vector3(1, 1, 1);
		float toY = checkpoint.transform.position.y - 20;
		transform.position = new Vector3(_startPosition.x, toY, _startPosition.z);
		//gameObject.SetActive(true);
	}
}