using UnityEngine;
using System.Collections;

public class RotatorCheckPointScript : MonoBehaviour
{
    Vector3 rotationVelocity = new Vector3(45, 0, 0);

    void FixedUpdate()
    {
        if (RotatingCirle.PointReached)
        {
            rotationVelocity = new Vector3(135, 0, 0);
        }
        transform.Rotate(rotationVelocity * Time.deltaTime);
    }
}