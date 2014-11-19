using UnityEngine;
using System.Collections;

public class PointBallon : MonoBehaviour
{
    public int PointsToAdd = 5;
    public AudioClip BaloonBurstSound;
    public SpriteRenderer Renderer;
    public GameObject balloonBurst;

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SimpleProjectile>() == null)   // Exit when not knife
            return;

        if (BaloonBurstSound != null)
            AudioSource.PlayClipAtPoint(BaloonBurstSound, transform.position, 0.4f);

        GameManager.Instance.AddPoints(PointsToAdd);
        GameManager.Instance.AddBalloon(1);
        Instantiate(balloonBurst, transform.position, transform.rotation);
        // Create the Floating text
        FloatingText.Show(string.Format("POP"), "PointsText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));

        // We only want the ballons once per level
        Destroy(gameObject);
    }
}
