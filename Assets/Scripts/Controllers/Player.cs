using UnityEngine;
using System.Collections;

// Handle the Input and tell the CharacterController2D what to do
public class Player : MonoBehaviour, ITakeDamage
{
    private bool _isFacingRight;
    private CharacterController2D _controller;
    private float _normalizedHorizontalSpeed;       // -1 Or 1 Moving to the left or to the right

    public bool IsCarringHay = false;
    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public float FireRate;
    public int MaxHealth = 100;
    public Animator Animator;
    public AudioClip
        PlayerDeathSound,
        PlayerHealthSound,
        PlayerHitSound,
        PlayerJumpSound,
        PlayerShootSound;
    public GameObject DamageEffect;
    public GameObject FireProjectileEffect;
    public Projectile Projectile;   // Public reference to a base type gameObject
    public Transform ProjectileLocation;

    public int Health { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsRespawning { get; private set; }

    private bool trampCol, isInHay = false;
    private float _canFireIn;

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;        // Sets the initial value of the bool depending on where the character stands in a new level
        Health = MaxHealth;
    }

    public void Update()
    {
        IsRespawning = false;
        _canFireIn -= Time.deltaTime;

        if (!IsDead)
            HandleInput();

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        if (IsDead)
            _controller.SetHorizontalForce(0);
        else
            _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));

        Animator.SetBool("IsGrounded", _controller.State.IsGrounded);
        Animator.SetBool("IsDead", IsDead);
        Animator.SetFloat("Speed", Mathf.Abs(_controller.Velocity.x) / MaxSpeed); // Dividing with MaxSpeed gives us a number from 0 to 1
    }

    public void FinishLevel()
    {
        // Disable player
        enabled = false;
        _controller.enabled = false;
        collider2D.enabled = false;
    }

    public void Kill()
    {
        AudioSource.PlayClipAtPoint(PlayerDeathSound, transform.position, 1.0f);

        _controller.HandleCollisions = false;   // fall through the world
        collider2D.enabled = false;             // no triggers
        IsDead = true;
        Health = 0;

        _controller.SetForce(new Vector2(0, 15));
    }

    public void RespawnAt(Transform spawnPoint)
    {
        IsRespawning = true;
        if (!_isFacingRight)
            Flip();

        IsDead = false;                         // Now he is alive
        _controller.HandleCollisions = true;    // enable collisions
        collider2D.enabled = true;
        Health = MaxHealth;

        _controller._overrideParameters = _controller.DefaultParameters;

        transform.position = spawnPoint.position;   // Spawn at spawnPoint which is specified
    }

    public void GiveHealth(int health, GameObject instagator)
    {
        AudioSource.PlayClipAtPoint(PlayerHealthSound, transform.position, 1.0f);
        // Create the Floating text
        FloatingText.Show(string.Format("+{0}!", health),
            "GotHealthText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));

        Health = Mathf.Min(Health + health, MaxHealth); // So that we can't have more than Max
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(PlayerHitSound, transform.position, 1.0f);

        FloatingText.Show(string.Format("-{0}", damage),
            "PlayerTakeDamageText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60));

        Instantiate(DamageEffect, transform.position, transform.rotation);
        Health -= damage;

        if (Health <= 0)
            LevelManager.Instance.KillPlayer();
    }

    private void HandleInput()
    {
        if (Time.timeScale == 1f)
        {
            if (Input.GetKey(KeyCode.D))
            {
                _normalizedHorizontalSpeed = 1;
                if (!_isFacingRight)
                    Flip();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _normalizedHorizontalSpeed = -1;
                if (_isFacingRight)
                    Flip();
            }
            else
            {
                _normalizedHorizontalSpeed = 0;
            }

            if (_controller.CanJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W)))
            {
                if (!trampCol)  // Jump if not colliding with trampoline
                {
                    AudioSource.PlayClipAtPoint(PlayerJumpSound, transform.position, 0.3f);
                    _controller.Jump();
                }
            }

            if (Input.GetMouseButtonDown(0))
                FireProjectile();

            if (Input.GetKey(KeyCode.E))
            {
                if (isInHay)
                    IsCarringHay = true;
                else
                    IsCarringHay = false;
            }
        }
    }

    private void FireProjectile()
    {
        if (_canFireIn > 0)
            return;

        if (FireProjectileEffect != null)
        {
            var effect = (GameObject)Instantiate(FireProjectileEffect, ProjectileLocation.position, ProjectileLocation.rotation);
            effect.transform.parent = transform; // Set the transform of the effect equal to the players, so that it follows 
        }

        var direction = _isFacingRight ? Vector2.right : -Vector2.right;

        var projectile = (Projectile)Instantiate(Projectile, ProjectileLocation.position, ProjectileLocation.rotation);
        projectile.Initialize(gameObject, direction, _controller.Velocity);

        _canFireIn = FireRate;

        AudioSource.PlayClipAtPoint(PlayerShootSound, transform.position, 1.0f);
        Animator.SetTrigger("Fire");
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // If colliding with trampoline
        if (other.gameObject.GetComponent<TrampolineAnimationScript>())
            trampCol = true;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Hay")
            isInHay = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        // If colliding with trampoline
        if (other.gameObject.GetComponent<TrampolineAnimationScript>())
            trampCol = false;

        if (other.tag == "Hay")
            isInHay = false;
    }
}