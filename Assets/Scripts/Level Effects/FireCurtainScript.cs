using UnityEngine;
using System.Collections;

public class FireCurtainScript : MonoBehaviour
{
	public GameObject fireCurtain;
	public Transform spawn;
	public float fireRate;
	public AudioClip sound;
	
	private float nextFire;

	void Update(){
		if (EndOfLevel3.elevate)
		{
			if (Time.time > nextFire) {
			
				nextFire = Time.time + fireRate;
				Instantiate(fireCurtain, spawn.position, spawn.rotation);
				AudioSource.PlayClipAtPoint(sound, transform.position, 0.3f);
			}
		}
	}
}