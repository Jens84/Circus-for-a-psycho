using UnityEngine;

public class SimpleProjectile : Projectile, ITakeDamage
{
    public GameObject DestroyedEffect;
    public int Damage;
    public int PointsToGivePlayer; // If the player destroyes the projectile, will get some points
    public float TimeToLive;

    public void Update()
    {
        if ((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }
        // move the projectile
        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    }

    // So that projectiles can be destroyed by other projectiles
    public void TakeDamage(int damage, GameObject instrigator)
    {
        // Debug.Log(PointsToGivePlayer + " - " +instrigator.name + " - " + gameObject.name);

        if (PointsToGivePlayer != 0)
        {
            var projectile = instrigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGivePlayer);
                FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer),
                    "PointBaconText",
                    new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }

        DestroyProjectile();
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    protected override void OnCollideOther(Collider2D other)
    {
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
