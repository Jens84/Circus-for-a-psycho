using UnityEngine;
using System.Collections;

public class EnableRenderer : MonoBehaviour
{

    private Player _player;

    public void Awake()
    {
        _player = GameObject.FindObjectOfType<Player>();
    }

    public void Update()
    {
        if (_player.IsCarringHay)
            renderer.enabled = true;
        else
            renderer.enabled = false;
    }
}
