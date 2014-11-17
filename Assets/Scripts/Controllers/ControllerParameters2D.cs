using System;
using UnityEngine;
using System.Collections;

[Serializable]  // To be able to change public variables from the inspector as this is not a Monobehaviour script
public class ControllerParameters2D     // Used for global / rewritable variables
{
    public enum JumpBehaviour
    { 
        CanJumpOnGround,
        CanJumpAnyWhere,
        CantJump
    }

    public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);

    [Range(0, 90)]                      // Slider for the Inspector
    public float SlopeLimit = 30;       // Limit for angles that we can climb

    public float Gravity = -30f;        // Our global gravity

    public JumpBehaviour JumpRestrictions;

    public float JumpFrequency = .25f;  // Limit how often we can jump

    public float JumpMagnitute = 16;    // How much force will be added to the y component
}
