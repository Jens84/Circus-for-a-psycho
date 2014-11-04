using System;
using UnityEngine;

public class StartScene : MonoBehaviour
{	
	public GUIStyle customGuiStyle, customPanelGuiStyle, customTitleGuiStyle, customTitle2GuiStyle;

	private int buttonWidth = 220;
	private int buttonHeight = 50;
	private int groupWidth = Screen.width;
	private int groupHeight = Screen.height;

	void Start()
	{
		Screen.lockCursor = false;
		Time.timeScale = 1;
		audio.Play ();
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(((Screen.width/2) - (groupWidth/2)), ((Screen.height/2) - (groupHeight/2)), groupWidth, groupHeight), customPanelGuiStyle);
		GUI.Button(new Rect(groupWidth/2 - 250, 100, 500, 100), "Circus", customTitleGuiStyle);
		GUI.Button(new Rect(groupWidth/2 - 300, 220, 600, 80), "for a Psycho", customTitle2GuiStyle);
		if(GUI.Button(new Rect((groupWidth/2) - (buttonWidth/2), (groupHeight* 0.6f), buttonWidth, buttonHeight), "Start", customGuiStyle))
		{
			audio.Stop ();
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect((groupWidth/2) - (buttonWidth/2), (groupHeight* 0.7f), buttonWidth, buttonHeight), "Quit", customGuiStyle))
		{
			Application.Quit();
		}
		GUI.EndGroup();

	}
}