using UnityEngine;
using System.Collections;

public class vanishingPlatform : MonoBehaviour
{
    public Color color;
    public float vanishTime;
    public float blinkingSpeed;

    private bool isBlinking;

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
        gameObject.SetActive(false);
    }
}
