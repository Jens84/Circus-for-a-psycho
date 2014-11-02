using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	public GUIText baconText, gameOverText, instructionsText;
	public float timeLeft = 5.0f;

	private static GUIManager instance;

	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
	}
	/* Let's add two static methods to GUIManager which our character can use to notify the GUI of changes to its bacon count. 
	 * Because the manager needs to use nonstatic variables in those methods, we add a static variable that references itself. 
	 * That way the static code can get to the component instance which actually has the gui text elements.*/
	public static void SetBacon(int bacon){
		instance.baconText.text = bacon.ToString();
	}

	void Update()
	{
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
	
	private void GameOver () {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
	}

	private void GameStart () {
		Screen.lockCursor = true;
		gameOverText.enabled = false;
		instructionsText.enabled = false;
		enabled = false;
	}

	void OnDestroy() {
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
	}
}
