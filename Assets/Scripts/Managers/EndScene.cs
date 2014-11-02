using System;
using UnityEngine;

public class EndScene : MonoBehaviour
{	
	public GUIText titleText, victoryText;

	private int buttonWidth = 200;
	private int buttonHeight = 50;
	private int groupWidth = 200;
	private int groupHeight = 170;		

	void Start()
	{
		Screen.lockCursor = false;
		Time.timeScale = 1;
		titleText.enabled = true;
		victoryText.enabled = true;
		audio.Play ();
	}
	
	void OnGUI()
	{
		GUI.BeginGroup(new Rect(((Screen.width/2) - (groupWidth/2)), ((Screen.height/2) - (groupHeight/2)), groupWidth, groupHeight));
		if(GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Start Game"))
		{
			audio.Stop ();
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect(0, 60, buttonWidth, buttonHeight), "Quit Game"))
		{
			audio.Stop ();
			Application.Quit();
		}
		GUI.EndGroup();
		
	}
}