using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleEnemyAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public float Speed = 8;
    public float FireRate = 1;
    public int StartingX = -1;
    public int MaxHealth = 200;
    public int PointsToGivePlayer = 50;
    public AudioClip FireSound;
    public AudioClip GetDamageSound;
    public GameObject DestroyedEffect;
    public GameObject DamageEffect;
    public Projectile Projectile;
    public Transform ProjectileLocation;

    public int Health { get; private set; }

    private CharacterController2D _controller;      // To have this player interact with our platforms
    private Vector2 _direction;                     // Flip everytime he hits an edge
    private Vector2 _startPosition;                 // Respawning
    private float _canFireIn;

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(StartingX, 0);            // Initializing moving to the left
        _startPosition = transform.position;        // Sets the initial value of the bool depending on where the character stands in a new level
        Health = MaxHealth;
    }

    public void Update()
    {
        if (Speed != 0)
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

            // Check if the player is close behind the lion
            var raycastBehind = Physics2D.Raycast(transform.position, -_direction, 10, 1 << LayerMask.NameToLayer("Player"));
            if (raycastBehind)
            {
                _direction = -_direction;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Reverse
            }

            // To check if the Player can be hit by this object
            var raycast = Physics2D.Raycast(transform.position, _direction, 15, 1 << LayerMask.NameToLayer("Player"));
            if (!raycast)
                return;

            // After this point the lion can "see" the player
            var projectile = (Projectile)Instantiate(Projectile, ProjectileLocation.position, ProjectileLocation.rotation);
            projectile.Initialize(gameObject, _direction, _controller.Velocity);
            _canFireIn = FireRate;

            if (FireSound != null)
                AudioSource.PlayClipAtPoint(FireSound, transform.position, 0.3f);
        }
    }

    // From ITakeDamage, to respond when he is hit by a projectile
    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                if (GetDamageSound != null)
                    AudioSource.PlayClipAtPoint(GetDamageSound, transform.position, 1.0f);

                FloatingText.Show(string.Format("-{0}", damage),
                    "PlayerTakeDamageText",
                    new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60));

                if (DamageEffect != null)
                    Instantiate(DamageEffect, transform.position, transform.rotation);

                Health -= damage;

                if (Speed != 0)
                {
                    if (Health <= 10)
                        LionFled();
                }
                else
                    if (Health <= 0)
                        KillStandingEnemy();
            }
        }
    }

    // From IPlayerRespawnListener, to respond when the player get reInstantiated
    public void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player)
    {
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }

    private void LionFled()
    {
        StartCoroutine(LionFledCo());
    }

    private IEnumerator LionFledCo()
    {
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        GameManager.Instance.AddPoints(PointsToGivePlayer);

        FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer),
            "PointBaconText",
            new CenteredTextPositioner(.3f));

        yield return new WaitForSeconds(0.5f);
        FloatingText.Show("The Lion fled the circus!", "CheckpointText", new CenteredTextPositioner(.2f));
        gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
    }

    private void KillStandingEnemy()
    {
        StartCoroutine(KillStandingEnemyCo());
    }

    private IEnumerator KillStandingEnemyCo()
    {
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        gameObject.SetActive(false);

        GameManager.Instance.AddPoints(PointsToGivePlayer);

        FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer),
            "PointBaconText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 5));

        yield return new WaitForSeconds(1f);
    }
}
