using UnityEngine;

public class GameHud : MonoBehaviour
{
    // Displaying stuff
    public GUISkin Skin;

    public void OnGUI()
    {
        GUI.skin = Skin;

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            GUILayout.BeginVertical(Skin.GetStyle("GameHud"));
            {
                GUILayout.Label(string.Format("Points: {0}", GameManager.Instance.Points), Skin.GetStyle("PointsText"));
                GUILayout.Label(string.Format("        : {0}", GameManager.Instance.Bacon), Skin.GetStyle("PointsText"));
                GUILayout.Label(string.Format("        : {0} / 5", GameManager.Instance.Balloon), Skin.GetStyle("PointsText"));

                /*var time = LevelManager.Instance.RunningTime;
                GUILayout.Label(string.Format(
                    "Extra {0} bonus points over time {1:00}:{2:00}",
                    LevelManager.Instance.CurrentTimeBonus,
                    time.Minutes + (time.Hours * 60),
                    time.Seconds),
                    Skin.GetStyle("TimeText"));*/
                // Fixed Layout
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }
}