using UnityEngine;
using System.Collections;

public class DeleteObject : MonoBehaviour
{

    public float time = 0.2f;

    void Awake()
    {
        Destroy(gameObject, time);
    }
}
