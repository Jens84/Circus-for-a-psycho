using UnityEngine;
using System.Collections;

public class FinalText : MonoBehaviour
{
    public GUIStyle customGuiStyle;
    public bool showText = true;

    private Rect textArea = new Rect(0, 0, Screen.width, Screen.height);

    void OnGUI()
    {
        if (showText)
        {
            GUI.Label(textArea, "Thank you for playing this game!", customGuiStyle);
        }
    }

    void Start()
    {
            StartCoroutine(_EndingGame());
    }

    private IEnumerator _EndingGame()  // Coroutine to murder the player
    {
        yield return new WaitForSeconds(8.0f);
        Application.LoadLevel("StartScene");
    }
}
