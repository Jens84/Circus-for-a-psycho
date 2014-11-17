using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour
{
    public float JumpMagnitude = 20;
    public AudioClip TrampolineSound;

    public void ControllerEnter2D(CharacterController2D controller)
    {
        if (TrampolineSound != null)
            AudioSource.PlayClipAtPoint(TrampolineSound, transform.position, 0.3f);

        controller.SetVerticalForce(JumpMagnitude);
    }
}
