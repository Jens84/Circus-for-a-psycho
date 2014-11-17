using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    private static readonly GUISkin Skin = Resources.Load<GUISkin>("GameSkin");

    public static FloatingText Show(string text, string style, IFloatingTextPositioner positioner)
    {
        var go = new GameObject("Floating Text");           // Instantiate a Floating Text gameobject from the class which is Monobehaviour
        var floatingText = go.AddComponent<FloatingText>(); // Instantiate the Floating Text component on the Floating Text gameObject
        floatingText.Style = Skin.GetStyle(style);
        floatingText._positioner = positioner;
        floatingText._content = new GUIContent(text);
        return floatingText;                    // Not used (Left here to allow changing the Text or Style varibles, after instantiation)
    }

    private GUIContent _content;
    private IFloatingTextPositioner _positioner;

    public string Text { get { return _content.text; } set { _content.text = value; } }
    public GUIStyle Style { get; set; }

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(_content); // Calculate the size of the content, meaning the text that is passed in the Show method
        if (!_positioner.GetPosition(ref position, _content, contentSize))  // If the position is correct
        {
            Destroy(gameObject);
            return;
        }

        GUI.Label(new Rect(position.x, position.y, contentSize.x, contentSize.y), _content, Style);
    }
}