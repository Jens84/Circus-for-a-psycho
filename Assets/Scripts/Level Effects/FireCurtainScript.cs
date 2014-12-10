using UnityEngine;
using System.Collections;

public class FireCurtainScript : MonoBehaviour
{
    public GameObject fireCurtain;
    public Transform spawn;
    public float fireRate;
    public AudioClip sound;

    private float nextFire;
    private SimpleEnemyAI _director;
    private Vector3 randomV;

    public void Awake()
    {
        _director = GameObject.FindObjectOfType<SimpleEnemyAI>();
    }

    void Update()
    {
        if (EndOfLevel3.elevate)
        {
            if (_director.Health > 0) // As long as the director is alive, spawn fire
            {
                if (Time.time > nextFire)
                {
                    randomV = new Vector3(Random.Range(-10, 10), 0, 0);
                    nextFire = Time.time + fireRate;
                    Instantiate(fireCurtain, spawn.position + randomV, spawn.rotation);
                    AudioSource.PlayClipAtPoint(sound, transform.position, 0.3f);
                }
            }
        }
    }
}