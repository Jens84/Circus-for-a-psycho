using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireControllerLevel3 : MonoBehaviour
{
    public float FireSpeed = 5;

    private Player _player;
    private Vector3 _startPosition;

    public void Awake()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _startPosition.y = _player.transform.position.y;
        _startPosition.x = transform.position.x;
    }

    public void FixedUpdate()
    {
        if (_player.IsRespawning)
        {
            transform.localScale = new Vector3(1, 1, 1);
            float toY = _player.transform.position.y;
            transform.position = new Vector3(_startPosition.x, toY, _startPosition.z);        
        }

        if (transform.position.y <= 205)
        {
            if (transform.position.y > 100)
                transform.Translate(0, 0.6f * FireSpeed * Time.deltaTime, 0);
            else
                transform.Translate(0, FireSpeed * Time.deltaTime, 0);
        }
    }
}