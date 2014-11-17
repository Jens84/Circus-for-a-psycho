using UnityEngine;

public class TrampolineAnimationScript : MonoBehaviour 
{
    public Animator Animator;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        Animator.SetTrigger("Activated");
	}
}
