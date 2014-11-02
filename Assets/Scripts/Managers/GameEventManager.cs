/* We make GameEventManager a static class that defines a GameEvent delegate type inside it. 
 * Note that the manager isn't a MonoBehaviour and won't be attached to any Unity object. */
public static class GameEventManager {
	
	public delegate void GameEvent();
	public static event GameEvent GameStart, GameOver, Restart;

	/* Care should be taken to only call an event if anyone is subscribed to it, 
	 * otherwise it will be null and the call will result in an error. */
	public static void TriggerGameStart(){
		if(GameStart != null){
			GameStart();
		}
	}

	public static void TriggerRestart(){
		if(Restart != null){
			Restart();
		}
	}

	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
		}
	}
}