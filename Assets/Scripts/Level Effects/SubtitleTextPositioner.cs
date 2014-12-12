using UnityEngine;

public class SubtitleTextPositioner : IFloatingTextPositioner
{
	public static Vector2 SubtitleSize;

	private readonly float _duration;
	private float _timePassed;
	
	public SubtitleTextPositioner(float duration)
	{
		_duration = duration;
	}
	
	public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
	{
		SubtitleSize = size;
		_timePassed += Time.deltaTime;
		if (_timePassed > _duration)
		{
			LevelManager.SubtitleBar.SetActive (false);
			return false;
		}
		// till the top of the screen
		position = new Vector2(Screen.width / 2f - size.x / 2f, 9 * Screen.height/10f);
		return true;    // Keep the text visible
	}
}