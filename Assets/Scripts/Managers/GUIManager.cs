using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	public GUIText baconText, gameOverText, titleText, instructionsText;
	public GUITexture baconTexture; 
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

	private void Restart () {
		float countdown = 3.0f;
		while (countdown > 0)
		{
			countdown -= Time.deltaTime;
		}
		GameEventManager.TriggerGameStart();
	}

	private void GameStart () {
		gameOverText.enabled = false;
		instructionsText.enabled = false;
		titleText.enabled = false;
		baconTexture.enabled = true;
		enabled = false;
	}
	
	/* Skipped to use seperate GUI texts
	public GUIStyle myGUIStyle;
	void OnGUI(){
		GUI.Box (new Rect (0,0,100,50), "Top-left", myGUIStyle);
		GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right", myGUIStyle);
		GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left", myGUIStyle);
		GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right", myGUIStyle);
		GUI.Box (new Rect (Screen.width/2,Screen.height/2,100,50), "GAME OVER", myGUIStyle);
	}*/
}
