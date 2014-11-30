using UnityEngine;

public class GiveDamageToPlayer : MonoBehaviour
{
    public int
        DamageToGive = 10,
        minDx = 5,
        maxDx = 10,
        minDy = 5,
        maxDy = 10;



    private Vector2
        _lastPosition,
        _velocity;          // To knock the player back with the velocity of the object

    public void LateUpdate() // To determine the velocity - will be perfomed after all other translations of Update
    {
        _velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime; // units per second
        _lastPosition = transform.position;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();  // To have this object interact with objects that have the player scipt attached
        if (player == null)                         // Skip for other than player objects
            return;

        player.TakeDamage(DamageToGive, gameObject);

        var controller = player.GetComponent<CharacterController2D>(); // To get the Velocity of the player from the CharacterController in order to have a correct knockback effect 
        var totalVelocity = controller.Velocity + _velocity;

        // (negate the velocity to knock the player back(-1)) then 
        // (get the sign from the total velocity (1 or -1 based on the direction the player is going), all directions knockback)
        // (with the clamp we are constraining the knockback direction, get rid of the sign and scale the factor)
        controller.SetForce(new Vector2(
            -1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 3, minDx, maxDx),
            -1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 3, minDy, maxDy)));
    }
}