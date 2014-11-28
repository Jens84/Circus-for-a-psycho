using UnityEngine;
using System.Collections;

public class SimpleGiraffeAI : MonoBehaviour
{
    public float Speed = 8;
    public int StartingX = -1;
    public AudioClip GiraffeSound;

    public bool IsFollowing { get; set; }

    private CharacterController2D _controller;      // To have this player interact with our platforms
    private Vector2 _direction;                     // Flip everytime he hits an edge
    private Vector2 _startPosition;                 // Respawning

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(StartingX, 0);            // Initializing moving to the left
        _startPosition = transform.position;        // Sets the initial value of the bool depending on where the character stands in a new level
    }

    public void Update()
    {
        if (IsFollowing)
        {
            _controller.SetHorizontalForce(_direction.x * Speed);
            // if moving to the left and collide to the left || etc
            if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
            {
                _direction = -_direction;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Reverse
            }

            // Check if the player is close behind the lion
            var raycastBehind = Physics2D.Raycast(transform.position, -_direction, 15, 1 << LayerMask.NameToLayer("Player"));
            if (raycastBehind)
            {
                _direction = -_direction;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Reverse
            }

            // To check if the Player can be hit by this object
            var raycast = Physics2D.Raycast(transform.position, _direction, 20, 1 << LayerMask.NameToLayer("Player"));
            if (!raycast)
                return;

            // After this point the giraffe can "see" the player
            if (GiraffeSound != null && GiraffeSound.isReadyToPlay)
                AudioSource.PlayClipAtPoint(GiraffeSound, transform.position, 0.3f);

            Player player = GameObject.FindObjectOfType<Player>();
        }
        else
        {
            _controller.SetHorizontalForce(0);
        }
    }
}