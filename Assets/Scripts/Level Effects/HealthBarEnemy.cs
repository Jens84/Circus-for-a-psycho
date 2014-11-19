using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    public SimpleEnemyAI Enemy;                                                       // Reference the player
    public Transform ForegroundSprite;                                          // To change the size of the sprite
    public SpriteRenderer ForegroundRenderer;                                   // Change the color of the health bar as it approaches zero
    public Color MaxHealthColor = new Color(63 / 255f, 255 / 255f, 63 / 255f);  // Translate the normal RGB to "Unity normal"
    public Color MinHealthColor = new Color(255 / 255f, 63 / 255f, 63 / 255f);


    public void Update()
    {
        var healthPercent = Enemy.Health / (float)Enemy.MaxHealth;            // Calculate the health% to scale the Sprite

        ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);         // Change the scale
        ForegroundRenderer.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthPercent);

        if (Enemy.MaxHealth > 20)
        {
            if (Enemy.Health <= 10)
                gameObject.SetActive(false);
        }
        else
            if (Enemy.Health <= 0)
                gameObject.SetActive(false);
    }
}