using UnityEngine;
using System.Collections;

public class AutoStartParticle : MonoBehaviour
{
    public ParticleSystem ImpactParticle;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        ImpactParticle.Play();
    }
}
