using UnityEngine;
using System.Collections;

public class vanishingPlatform : MonoBehaviour, IPlayerRespawnListener
{
    public Color color;
    public float vanishTime;
    public float blinkingSpeed;

    private bool isBlinking;
    private Color _startColor;
    private Vector3
        _startPosition,
        _startScale;

    public void Awake()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
        _startColor = renderer.material.color;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !isBlinking)
        {
            StartCoroutine("Blinking");
            StartCoroutine("Destroying");
        }
    }

    IEnumerator Blinking()
    {
        isBlinking = true;
        Color c = color;
        float increment = -0.25F;

        while (true)
        {
            if (c.a == 0 || c.a == 1)
                increment = -increment;

            c.a -= increment;
            renderer.material.SetColor("_Color", c);
            yield return new WaitForSeconds(blinkingSpeed);
        }
    }

    IEnumerator Destroying()
    {
        yield return new WaitForSeconds(vanishTime);
        gameObject.renderer.enabled = false;
        gameObject.collider2D.enabled = false;
    }

    // From IPlayerRespawnListener, to respond when the player get reInstantiated
    public void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player)
    {
        isBlinking = false;
        StopCoroutine("Blinking");
        StopCoroutine("Destroying");
        transform.localScale = _startScale;
        transform.position = _startPosition;
        gameObject.renderer.enabled = true;
        renderer.material.SetColor("_Color", _startColor);
        gameObject.collider2D.enabled = true;
    }
}
