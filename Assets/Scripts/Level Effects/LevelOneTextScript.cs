using UnityEngine;
using System.Collections;

public class LevelOneTextScript : MonoBehaviour {

	public AudioClip CollectTheBaconSound;

	void Awake () {
		audio.PlayOneShot(CollectTheBaconSound);
		Destroy (gameObject, 2.0f);
	}
}