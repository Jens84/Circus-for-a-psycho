using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public float Speed;
    public float FireRate = 1;
    public Projectile Projectile;
    public GameObject DestroyedEffect;
    public int PointsToGivePlayer = 50;
    public AudioClip FireSound;

    private CharacterController2D _controller;      // To have this player interact with our platforms
    private Vector2 _direction;                     // Flip everytime he hits an edge
    private Vector2 _startPosition;                 // Respawning
    private float _canFireIn;

    public void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);    // Initializing moving to the left
        _startPosition = transform.position;
    }

    public void Update()
    {
        _controller.SetHorizontalForce(_direction.x * Speed);
        // if moving to the left and collide to the left || etc
        if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Reverse
        }

        if ((_canFireIn -= Time.deltaTime) > 0)
            return;

        // To check if the Player can be hit by this object
        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));
        if (!raycast)
            return;

        var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate;

        if (FireSound != null)
            AudioSource.PlayClipAtPoint(FireSound, transform.position, 0.3f);
    
    }

    // From ITakeDamage, to respond when he is hit by a projectile
    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGivePlayer);
                FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer),
                    "PointBaconText",
                    new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }

        Instantiate(DestroyedEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    // From IPlayerRespawnListener, to respond when the player get reInstantiated
    public void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player)
    {
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }
}
