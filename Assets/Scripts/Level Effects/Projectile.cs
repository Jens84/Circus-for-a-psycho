using UnityEngine;

// Flexible technic - abstact ==> base type
public abstract class Projectile : MonoBehaviour
{
    public float Speed;
    public LayerMask CollisionMask;

    public GameObject Owner { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector2 InitialVelocity { get; private set; }

    // MonoBehaviour cannot have constructors, instead we make our Initialize
    public void Initialize(GameObject owner, Vector2 direction, Vector2 initialVelocity)
    {
        // Flip/orient the projectile
        transform.right = direction;
        
        Owner = owner;
        Direction = direction;
        InitialVelocity = initialVelocity;
        OnInitialized();
    }

    // protected = only accessible from the children
    // virtual = overwritable from the children / optional
    protected virtual void OnInitialized()
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        /*  Mask collision check explanation
         * 
         *  Layer #     Binary      Decimal
         *  Layer 0 =   0000 0001 = 1
         *  Layer 1 =   0000 0010 = 2 
         *  Layer 2 =   0000 0100 = 4
         *  Layer 3 =   0000 1000 = 8
         *  Layer 4 =   0001 0000 = 16
         *  Layer 5 =   0010 0000 = 32
         *  Layer 6 =   0100 0001 = 64
         *  Layer 7 =   1000 0001 = 128
         *
         * other.gameObject.layer   = the integer number(#) of the Layer
         * LayerMask.value          = the Binary representation of the combination of the layers
         * e.g. combined layers:        ||Layer1Layer2 Layer6Layer5|| 
         * would be in 8 bits (byte)     0110 0110
         * 
         * so if we want to ask: Is Layer 5 in the mask?
         * would be (1 << 5)         * 
         * the operator << takes the bits in a number and shifts it left X=5 amount of times
         * 0000 0001 << 5 = 0010 0000
         * 
         * 0110 0110
         * & (binary and)
         * 0010 0000
         * ----------
         * 0010 0000 != 0
         * 
         * so if we checked for Layer 7 that in not in the Mask would be
         * 0110 0110
         * & (binary and)
         * 1000 0000
         * ----------
         * 0000 0000 == 0
         */
        // Check for Mask collision
        if ((CollisionMask.value & (1 << other.gameObject.layer)) == 0)
        {
            OnNotCollideWith(other);
            return;
        }

        // Check for Owner collision
        var isOwner = other.gameObject == Owner;
        if (isOwner)
        {
            OnCollideOwner();
            return;
        }

        // Check for collision with objects that can take damage
        // using the Non generic form of GetComponent as interfaces do not inherit from UnityEngine.Object
        var takeDamage = (ITakeDamage)other.GetComponent(typeof(ITakeDamage));
        if (takeDamage != null)
        {
            OnCollideTakeDamage(other, takeDamage);
            return;
        }

        // Check for any other collisions
        OnCollideOther(other);
    }

    // Children can optionally add a behaviour if there is a collision not matching the CollisionMask
    protected virtual void OnNotCollideWith(Collider2D other)
    {
    }

    protected virtual void OnCollideOwner()
    {
    }

    protected virtual void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
    }

    protected virtual void OnCollideOther(Collider2D other)
    {
    }
}