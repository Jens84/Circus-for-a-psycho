using UnityEngine;
using System.Collections;

public class RotatingCirle : MonoBehaviour
{

    public static bool PointReached = false;
    public GameObject checkPointVFX;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<Player>() == null)
            return;

        PointReached = true;

        if (checkPointVFX == null)
            return;

        Instantiate(checkPointVFX, transform.position, transform.rotation);
        Destroy(gameObject, 2.0f);
    }
}
