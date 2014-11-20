using System;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GUIStyle
        customGuiStyle,
        customToggleOnGuiStyle,
        customToggleSwapGuiStyle,
        customPanelGuiStyle,
        customPauseGuiStyle;

    private bool paused = false;
    private bool muteToggle = false;
    private int buttonWidth = 220;
    private int buttonHeight = 50;
    private int groupWidth = 500;
    private int groupHeight = 400;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Screen.lockCursor)
                Screen.lockCursor = false;
            else
                Screen.lockCursor = true;
            paused = togglePause();
        }
    }

    void OnGUI()
    {
        if (paused)
        {
            GUI.BeginGroup(new Rect(((Screen.width / 2) - (groupWidth / 2)), ((Screen.height / 2) - (groupHeight / 2)), groupWidth, groupHeight), customPanelGuiStyle);
            GUI.TextArea(new Rect((groupWidth / 2) - (buttonWidth / 2), (groupHeight * 0.2f), buttonWidth, buttonHeight), "Pause", customPauseGuiStyle);
            if (GUI.Button(new Rect((groupWidth / 2) - (buttonWidth / 2), (groupHeight * 0.5f), buttonWidth, buttonHeight), "Main Menu", customGuiStyle))
            {
                Application.LoadLevel(0);
            }
            if (GUI.Button(new Rect((groupWidth / 2) - (buttonWidth / 2), (groupHeight * 0.6f), buttonWidth, buttonHeight), "Restart Game", customGuiStyle))
            {
                Application.LoadLevel(1);
            }
            if (GUI.Button(new Rect((groupWidth / 2) - (buttonWidth / 2), (groupHeight * 0.7f), buttonWidth, buttonHeight), "Quit Game", customGuiStyle))
            {
                Application.Quit();
            }
            muteToggle = GUI.Toggle(new Rect((groupWidth * 0.8f), (groupHeight * 0.8f), 100, 100), muteToggle, "Mute", customToggleSwapGuiStyle);
            GUI.EndGroup();

            if (!muteToggle)
            {// If false
                AudioListener.volume = 1;
                customToggleSwapGuiStyle = customGuiStyle;
            }
            else
            {
                AudioListener.volume = 0;
                customToggleSwapGuiStyle = customToggleOnGuiStyle;
            }
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}