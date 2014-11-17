using UnityEngine;
using System.Collections;

public class GiveHealth : MonoBehaviour, IPlayerRespawnListener
{

    public GameObject Effect;
    public int HealthToGive = 10;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)   // Exit when not player
            return;

        player.GiveHealth(HealthToGive, gameObject);
        Instantiate(Effect, transform.position, transform.rotation);

        gameObject.SetActive(false); // we dont want to destroy our objects
    }

    // using the Interface IPlayerRespawnListener to keep track items in between checkpoints
    public void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player)
    {
        gameObject.SetActive(true);
    }
}
