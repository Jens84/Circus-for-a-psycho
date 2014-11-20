using UnityEngine;
using System.Collections;

// Handles gravity, collision etc
public class CharacterController2D : MonoBehaviour
{
    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 10;
    private const int TotalVerticalRays = 4;

    public static bool PlayerJumped;
    private static readonly float SlopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);

    public ControllerParameters2D DefaultParameters;
    public ControllerParameters2D _overrideParameters;
    public LayerMask PlatformMask;

    // Properties
    public bool HandleCollisions { get; set; }
    public ControllerState2D State { get; private set; }
    public ControllerParameters2D Parameters { get { return _overrideParameters ?? DefaultParameters; } }   // If _override is null return default
    public GameObject StandingOn { get; private set; }
    public Vector2 Velocity { get { return _velocity; } }
    public Vector3 PlatformVelocity { get; private set; }

    public bool CanJump
    {
        get
        {
            if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehaviour.CanJumpAnyWhere)        // We can jump based on the restrictions and _jumpIn <= 0
                return _jumpIn <= 0;
            // Or when we are grounded
            if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehaviour.CanJumpOnGround)
                return State.IsGrounded;

            return false;
        }
    }

    // Private fields
    private float _jumpIn;
    private BoxCollider2D _boxCollider;
    private GameObject _lastStandingOn;
    private Transform _transform;
    private Vector2 _velocity;
    private Vector3 _localScale;

    private Vector3
        _activeGlobalPlatformPoint,
        _activeLocalPlatformPoint;

    private Vector3
        _rayCastTopLeft,
        _rayCastBottomRight,
        _rayCastBottomLeft;

    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays;


    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState2D();    // So that our player script can reference the state
        _transform = transform;
        _localScale = transform.localScale;
        _boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        _horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        var colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);
    }

    public void AddForce(Vector2 force)
    {
        _velocity += force;
    }

    public void SetForce(Vector2 force)
    {
        _velocity = force;
    }

    public void SetHorizontalForce(float x)
    {
        // Velocity.x = x; /// !!! Wrong as Velocity is not a variable but a value type with getters and setters (properties (so we are always working with copies))
        _velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }

    public void Jump()
    {
        PlayerJumped = true;
        AddForce(new Vector2(0, Parameters.JumpMagnitute));
        _jumpIn = Parameters.JumpFrequency;      // To determine if the played can jump or not 
    }

    public void LateUpdate()     // So we are sure that all other movements are applied
    {
        _jumpIn -= Time.deltaTime;

        _velocity.y += Parameters.Gravity * Time.deltaTime;     // Gravity
        Move(Velocity * Time.deltaTime);
    }

    private void Move(Vector2 deltaMovement)
    {
        bool wasGrounded = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            HandlePlatforms();
            CalculateRayOrigins();

            if (deltaMovement.y < 0 && wasGrounded)
                HandleVerticalSlope(ref deltaMovement);

            if (Mathf.Abs(deltaMovement.x) > .001f)
                MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement);
            CorrectHorizontalPlacement(ref deltaMovement, true);    // to the right
            CorrectHorizontalPlacement(ref deltaMovement, false);   // to the left
        }

        _transform.Translate(deltaMovement, Space.World);

        if (Time.deltaTime > 0)
            _velocity = deltaMovement / Time.deltaTime;

        _velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingUpSlope)
            _velocity.y = 0;

        if (StandingOn != null)
        {
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

            // Used for sending messages to other platforms
            #region Messages
            if (_lastStandingOn != StandingOn)
            {
                if (_lastStandingOn != null)
                    _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);

                StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                _lastStandingOn = StandingOn;
            }
            else if (StandingOn != null)
                StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
        }
        else if (_lastStandingOn != null)
        {
            _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
            _lastStandingOn = null;
        }
            #endregion Messages
    }

    private void HandlePlatforms()      // To calculate the velocity of the things we are standing on
    {
        if (StandingOn != null)
        {
            var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
            var moveDistance = newGlobalPlatformPoint - _activeGlobalPlatformPoint;

            if (moveDistance != Vector3.zero)
                transform.Translate(moveDistance, Space.World);

            PlatformVelocity = (newGlobalPlatformPoint - _activeGlobalPlatformPoint) / Time.deltaTime;   // To add the velocity of the platform to our jump eg
        }
        else
            PlatformVelocity = Vector3.zero;

        StandingOn = null;
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)        // Used to correct the horizontal placement of the Player, when another object is moving towards him
    {
        var halfWidth = (_boxCollider.size.x * _localScale.x) / 2f;
        var rayOrigin = isRight ? _rayCastBottomRight : _rayCastBottomLeft;

        if (isRight)        // The origin of the rays is in the center of the player now
            rayOrigin.x -= (halfWidth - SkinWidth);
        else
            rayOrigin.x += (halfWidth - SkinWidth);

        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0f;

        for (int i = 1; i < TotalHorizontalRays - 1; i++)
        {
            var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));       // Taking into account the change of the y axes for the calculation
            Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
            if (!raycastHit)
                continue;

            // Is set to something when we are pushed away by a platform
            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
        }

        deltaMovement.x += offset;
    }

    private void CalculateRayOrigins()      // To precompute where the Rays will be shot from
    {
        var size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        var center = new Vector2(_boxCollider.center.x * _localScale.x, _boxCollider.center.y * _localScale.y);

        _rayCastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        _rayCastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        _rayCastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;                   // How far we are projectiling the ray
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;           // Depending on player's direction
        var rayOrigin = isGoingRight ? _rayCastBottomRight : _rayCastBottomLeft;    // Where we start shooting our rays

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));     // How many rays we should have
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);        // Draw a ray from the rayVector and then the rayDirection scaled up by the distance

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);         // Did we hit anything?
            if (!rayCastHit)                                                                                // If yes, continue the loop
                continue;

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;

            deltaMovement.x = rayCastHit.point.x - rayVector.x;     // The furthest we can move without hitting an obstactle
            rayDistance = Mathf.Abs(deltaMovement.x);               // The rayDistance will change based on the previous rayCast in every loop

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
            }

            if (rayDistance < SkinWidth + .0001f)
                break;
        }
    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;                   // How far we are projectiling the ray
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;                    // Depending on player's direction
        var rayOrigin = isGoingUp ? _rayCastTopLeft : _rayCastBottomLeft;           // Where we start shooting our rays

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;
        for (var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);     // How many rays we should have
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);        // Draw a ray from the rayVector and then the rayDirection scaled up by the distance

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);         // Did we hit anything?
            if (!rayCastHit)                                                                                // If yes, continue the loop
                continue;

            // Keep track on where we are standing on
            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - rayCastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    StandingOn = rayCastHit.collider.gameObject;
                }
            }

            deltaMovement.y = rayCastHit.point.y - rayVector.y;     // The furthest we can move up or down without hitting an obstacle
            rayDistance = Mathf.Abs(deltaMovement.y);               // The rayDistance will change based on the previous rayCast in every loop

            if (isGoingUp)
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;         // We are on the ground
            }

            // Slopes
            if (!isGoingUp && deltaMovement.y > .0001f)
                State.IsMovingUpSlope = true;

            if (rayDistance < SkinWidth + 0.0001f)
                break;
        }
    }

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        if (Mathf.RoundToInt(angle) == 90)      // Don't move on angle 90
            return false;

        if (angle > Parameters.SlopeLimit)      // If the slope is too big
        {
            deltaMovement.x = 0;
            return true;
        }

        if (deltaMovement.y > .07f)             // Moving up-ish
            return true;

        deltaMovement.x += (isGoingRight ? -SkinWidth : SkinWidth);
        deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);

        // Now we know we are moving up a slope and that we are colliding bellow
        State.IsMovingUpSlope = true;
        State.IsCollidingBelow = true;
        return true;
    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
        var center = (_rayCastBottomLeft.x + _rayCastBottomRight.x) / 2;
        var direction = -Vector2.up;        // Down

        var slopeDistance = SlopeLimitTangant * (_rayCastBottomRight.x - center);
        var slopeRayVector = new Vector2(center, _rayCastBottomLeft.y);

        // Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);
        var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);
        if (!raycastHit)
            return;

        var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
        if (!isMovingDownSlope)
            return;

        var angle = Vector2.Angle(raycastHit.normal, Vector2.up);
        if (Mathf.Abs(angle) < .0001f) // This means we a not on a slope, that we are on sth perpendicular to us which is wrong, so we return
            return;

        State.IsMovingDownSlope = true;
        State.SlopeAngle = angle;
        deltaMovement.y = raycastHit.point.y - slopeRayVector.y;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<ControllerPhysicsVolume2D>();
        if (parameters == null)
            return;

        _overrideParameters = parameters.Parameters;    // Here we overrive the default parameters with the trigger's parameters
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<ControllerPhysicsVolume2D>();
        if (parameters == null)
            return;

        _overrideParameters = null;    // When we exit the volume we return it to null
    }
}
