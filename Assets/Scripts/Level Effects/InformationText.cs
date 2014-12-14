using UnityEngine;

public class InformationText : MonoBehaviour
{
    public string[] Text;
    public AudioClip InfoSound;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        if (InfoSound != null)
            AudioSource.PlayClipAtPoint(InfoSound, transform.position, 1f);

        LevelManager.Instance.DisplayInfoText(Text);

        gameObject.SetActive(false);
    }
}

