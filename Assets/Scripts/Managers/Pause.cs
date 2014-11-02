using System;
using UnityEngine;

public class Pause : MonoBehaviour
{	
	private int buttonWidth = 200;
	private int buttonHeight = 50;
	private int groupWidth = 200;
	private int groupHeight = 170;
	bool paused = false;	

	void Start()
	{
		Screen.lockCursor = false;
		Time.timeScale = 1;
	}

	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) 
		{
			Screen.lockCursor = false;
			paused = togglePause ();
		}
	}

	void OnGUI()
	{
		if (paused) 
		{
			GUI.BeginGroup(new Rect(((Screen.width/2) - (groupWidth/2)), ((Screen.height/2) - (groupHeight/2)), groupWidth, groupHeight));
         	if(GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Main Menu"))
         	{
				Application.LoadLevel(0);
			}
			if(GUI.Button(new Rect(0, 60, buttonWidth, buttonHeight), "Restart Game"))
			{
				Application.LoadLevel(1);
			}
			if(GUI.Button(new Rect(0, 120, buttonWidth, buttonHeight), "Quit Game"))
			{
				Application.Quit();
			}
			GUI.EndGroup();
		}
	}

	bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
			return(false);
		}
		else
		{
			Time.timeScale = 0f;
			return(true);
		}
	}
}