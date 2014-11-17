using System;
using UnityEngine;
using System.Collections;

public class ControllerState2D // Keeping track the state of the controller through properties for safety reasons
{
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }      // Hitting a ceiling?
    public bool IsMovingDownSlope { get; set; }     // Are we on the ground?
    public bool IsMovingUpSlope { get; set; }
    public bool IsGrounded { get { return IsCollidingBelow; } }
    public float SlopeAngle { get; set; }

    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }

    public void Reset() 
    {
        IsMovingUpSlope =
            IsMovingUpSlope =
            IsCollidingLeft =
            IsCollidingRight =
            IsCollidingAbove =
            IsCollidingBelow = false;
    }

    public override string ToString()       // Used for debuging
    {
        // return base.ToString();
        return string.Format(
            "(controller: r: {0} l: {1} a: {2} b: {3} down-slope: {4} up-slope:{5} angle: {6})",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow,
            IsMovingDownSlope,
            IsMovingUpSlope,
            SlopeAngle); 
    }
}