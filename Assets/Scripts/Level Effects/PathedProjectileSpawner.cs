using UnityEngine;

public class PathedProjectileSpawner : MonoBehaviour
{
    public Transform Destination;
    public PathedProjectile Projectile; // prefab with a component of specific type

    public GameObject SpawnEffect;
    public float Speed;
    public float FireRate;
    public Quaternion Rotation;
    public AudioClip ProjectileSound;
    public Animator Animator;

    private float _nextShotInSeconds;

    public void Start()
    {
        _nextShotInSeconds = FireRate;
    }

    public void Update()
    {
        if ((_nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        _nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, Rotation);
        projectile.Initialize(Destination, Speed);

        if (SpawnEffect != null)
            Instantiate(SpawnEffect, transform.position, transform.rotation);

        if (ProjectileSound != null)
            AudioSource.PlayClipAtPoint(ProjectileSound, transform.position, 0.3f);

        if (Animator != null)
            Animator.SetTrigger("Fire");
    }

    // Draw a line of the projectile in the scene
    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
