using UnityEngine;
using System.Collections;

public class SimpleGiraffeAI : MonoBehaviour
{
    public float Speed = 8;
    public AudioClip GiraffeSound;

    public bool IsFollowing { get; set; }

    private CharacterController2D _controller;      // To have this player interact with our platforms
    private Vector2 _direction;                     // Flip everytime he hits an edge
    private Player _player;
    private bool _gReturning;

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);            // Initializing looking to the left
        _player = GameObject.FindObjectOfType<Player>();
        _gReturning = false;
    }

    public void FixedUpdate()
    {
        if (!_gReturning)
        {
            if (_player.IsCarringHay)
            {
                // Check if the player is close behind the giraffe
                var raycastBehind = Physics2D.Raycast(transform.position, -_direction, 15, 1 << LayerMask.NameToLayer("Player"));
                if (raycastBehind)
                {
                    _direction = -_direction;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                // If infront continue
                var raycastFront = Physics2D.Raycast(transform.position, _direction, 15, 1 << LayerMask.NameToLayer("Player"));
                if (!raycastFront)
                    return;

                // After this point the giraffe can "see" the player
                // if moving to the left and collide to the left || etc
                if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
                    _direction = -_direction;

                if (GiraffeSound != null && GiraffeSound.isReadyToPlay)
                    AudioSource.PlayClipAtPoint(GiraffeSound, transform.position, 0.3f);

                _controller.SetHorizontalForce(_direction.x * Speed);

                if (!_controller.State.IsCollidingLeft && !_controller.State.IsCollidingRight)
                    if (_controller.Velocity.x > 0)
                        transform.localScale = new Vector3((_direction.x * transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                _controller.SetHorizontalForce(0);
            }
        }
        else
        {
            if ((Mathf.Abs(transform.localPosition.x) < 70))
            {
                _gReturning = false;
                _controller.SetHorizontalForce(0);
            }
            else
            {
                _controller.SetHorizontalForce(-Speed);
            }
        }
    }

    public void gReturn()
    {
        StartCoroutine(gReturnlCo());
    }

    private IEnumerator gReturnlCo()
    {
        yield return new WaitForSeconds(10f);
        _gReturning = true;
        StopCoroutine(gReturnlCo());
    }
}