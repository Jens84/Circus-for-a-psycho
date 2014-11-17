using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Checkpoint2D : MonoBehaviour
{
    private List<IPlayerRespawnListener> _listeners;

    public void Awake()
    {
        _listeners = new List<IPlayerRespawnListener>();
    }

    public void PlayerHitCheckpoint() // to be invoked by the LevelManager when the player hits the checkpoint
    {
        StartCoroutine(PlayerHitCheckpointCo(LevelManager.Instance.CurrentTimeBonus));
    }

    public void PlayerLeftCheckpoint()
    {
    }

    private IEnumerator PlayerHitCheckpointCo(int bonus)  // Coroutine to make sure that our text won't change
    {
        FloatingText.Show("Checkpoint!", "CheckpointText", new CenteredTextPositioner(.3f));
        yield return new WaitForSeconds(0.5f);
        FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckpointText", new CenteredTextPositioner(.3f));
    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);  // respawn the player to the transform of the checkpoint
        foreach (var listener in _listeners)
            listener.OnPlayerRespawnInThisCheckPoint(this, player);
    }

    public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
    {
        _listeners.Add(listener);
    }
}