using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Player Player;                                                       // Reference the player
    public Transform ForegroundSprite;                                          // To change the size of the sprite
    public SpriteRenderer ForegroundRenderer;                                   // Change the color of the health bar as it approaches zero
    public Color MaxHealthColor = new Color(63 / 255f, 255 / 255f, 63 / 255f);  // Translate the normal RGB to "Unity normal"
    public Color MinHealthColor = new Color(255 / 255f, 63 / 255f, 63 / 255f);


    public void Update()
    {
        var healthPercent = Player.Health / (float)Player.MaxHealth;            // Calculate the health% to scale the Sprite

        ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);         // Change the scale
        ForegroundRenderer.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthPercent);
    }
}