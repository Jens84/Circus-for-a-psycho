using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	float timeLeft = 5.0f;
	bool GameOver = false;

	public GUIStyle myGUIStyle;

	void OnGUI(){

		GUI.Box (new Rect (0,0,100,50), "Top-left", myGUIStyle);
		GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right", myGUIStyle);
		GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left", myGUIStyle);
		GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right", myGUIStyle);

		if(GameOver)
		{
			GUI.Box (new Rect (Screen.width/2,Screen.height/2,100,50), "GAME OVER", myGUIStyle);
		}

	}

	void Update()
	{
		timeLeft -= Time.deltaTime;
		if(timeLeft < 0)
		{
			GameOver = true;
		}
	}
}
