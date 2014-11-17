﻿using UnityEngine;
using System.Collections;

public class PointBacon : MonoBehaviour, IPlayerRespawnListener
{
    public GameObject Effect;
    public int PointsToAdd = 10;
    public AudioClip BaconEatSound;
    public Animator Animator;
    public SpriteRenderer Renderer;

    private bool _isCollected;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected)
            return;

        if (other.GetComponent<Player>() == null)   // Exit when not player
            return;

        if (BaconEatSound != null)
            AudioSource.PlayClipAtPoint(BaconEatSound, transform.position, 0.3f);

        GameManager.Instance.AddPoints(PointsToAdd);
        Instantiate(Effect, transform.position, transform.rotation);
        // Create the Floating text
        FloatingText.Show(string.Format("+{0}!", PointsToAdd), "PointBaconText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));

        _isCollected = true;
        Animator.SetTrigger("Collect");
    }

    // Called from animation (Add Event.)
    public void FinishAnimationEvent()
    {
        Renderer.enabled = false; // we dont want to destroy our objects
        Animator.SetTrigger("Reset");
    }

    // using the Interface IPlayerRespawnListener to keep track items in between checkpoints
    public void OnPlayerRespawnInThisCheckPoint(Checkpoint2D checkpoint, Player player)
    {
        _isCollected = false;
        Renderer.enabled = true;
    }
}
