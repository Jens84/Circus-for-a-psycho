using UnityEngine;

public class PathedProjectile : Projectile, ITakeDamage
{
    private Transform _destination;
    private float _speed;

    public GameObject DestroyEffect;
    public int PointsToGivePlayer = 0;
    public AudioClip DestroySound;

    public void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
        if (distanceSquared > .01f * .01f)
            return;

        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);

        if (DestroySound != null)
            AudioSource.PlayClipAtPoint(DestroySound, transform.position, 0.3f);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);

        var projectile = instigator.GetComponent<Projectile>();
        if (projectile != null && projectile.Owner.GetComponent<Player>() != null && PointsToGivePlayer != 0)
        {
            GameManager.Instance.AddPoints(PointsToGivePlayer);
            FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer),
                "PointBaconText",
                new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
        }
    }

    protected override void OnCollidePlayer(Collider2D other)
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        var player = other.GetComponent<Player>();  // To have this object interact with objects that have the player scipt attached
        if (player != null)                         // Skip for other than player objects
            Destroy(gameObject);
    }
}