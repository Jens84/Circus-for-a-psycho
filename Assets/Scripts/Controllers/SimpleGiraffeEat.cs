using UnityEngine;
using System.Collections;

public class SimpleGiraffeEat : MonoBehaviour
{
    public Animator Animator;

    private SimpleGiraffeAI _giraffe;

    public void Start()
    {
        _giraffe = FindObjectOfType<SimpleGiraffeAI>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();  // To have this object interact with objects that have the player scipt attached
        if (player == null)                         // Skip for other than player objects
            return;

        if (player.IsCarringHay)
        {
            Animator.SetTrigger("Eat");
            player.DestroyHay();
            _giraffe.gReturn();
        }
    }
}
