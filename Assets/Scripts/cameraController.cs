using UnityEngine;
using System.Collections;

public class screenKill : MonoBehaviour {
	public float speed;
	
	void FixedUpdate(){
		transform.Translate(speed * Time.deltaTime, 0, 0);
	}
}