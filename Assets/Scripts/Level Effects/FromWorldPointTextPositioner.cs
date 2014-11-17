using UnityEngine;

public class FromWorldPointTextPositioner : IFloatingTextPositioner
{
    private readonly Camera _camera;            // used to calculate where in the screen should the text be, based on its world coordinates
    private readonly Vector3 _worldPosition;
    private readonly float _speed;
    private float _timeToLive;
    private float _yOffset;

    public FromWorldPointTextPositioner(Camera camera, Vector3 worldPosition, float timeToLive, float speed)
    {
        _camera = camera;
        _worldPosition = worldPosition;
        _timeToLive = timeToLive;
        _speed = speed;
    }

    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
    {
        if ((_timeToLive -= Time.deltaTime) <= 0)
            return false;

        var screenPosition = _camera.WorldToScreenPoint(_worldPosition);
        position.x = screenPosition.x - (size.x / 2);   // Center the text on the world position based on its size in pixels
        position.y = Screen.height - screenPosition.y - _yOffset;

        _yOffset += Time.deltaTime * _speed;
        return true;    // we still want the text to be visible
    }
}