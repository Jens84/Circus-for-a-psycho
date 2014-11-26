using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip SoundToPlayOnce;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // If colliding with player
        if (other.GetComponent<Player>())
            AudioSource.PlayClipAtPoint(SoundToPlayOnce, transform.position, 1.0f);
    }
}
