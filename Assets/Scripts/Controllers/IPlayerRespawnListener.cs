public interface IPlayerRespawnListener
{
    void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player);   // To be invoked by any object that implements this interface, when the player gets respawned in the checkPoint the object currently is under
}
