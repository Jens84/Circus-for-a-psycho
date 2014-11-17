using UnityEngine;

public class CenteredTextPositioner : IFloatingTextPositioner
{
    private readonly float _speed;
    private float _textPosition;

    public CenteredTextPositioner(float speed)
    {
        _speed = speed;
    }

    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
    {
        _textPosition += Time.deltaTime * _speed;
        if (_textPosition > 1)
            return false;

        // till the top of the screen
        position = new Vector2(Screen.width / 2f - size.x / 2f, Mathf.Lerp(Screen.height / 2f + size.y, 0, _textPosition));
        return true;    // Keep the text visible
    }
}
