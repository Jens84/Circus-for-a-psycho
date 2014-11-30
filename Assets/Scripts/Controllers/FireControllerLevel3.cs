using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireControllerLevel3 : MonoBehaviour
{
    public float FireSpeed = 2;

    private Player _player;
    private Vector3 _startPosition;

    public void Awake()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _startPosition.y = _player.transform.position.y;
        _startPosition.x = transform.position.x;
    }

    public void Update()
    {
        if (_player.IsRespawning)
        {
            transform.localScale = new Vector3(1, 1, 1);
            float toY = _player.transform.position.y;
            transform.position = new Vector3(_startPosition.x, toY, _startPosition.z);
        }
    }

    public void FixedUpdate()
    {
        if (_player.IsDead)
        {
            transform.Translate(0, 0, 0);
        }
        else if (transform.position.y <= 205) // Top
        {
            if (transform.position.y > 100)
                transform.Translate(0, 0.7f * FireSpeed * Time.deltaTime, 0);
            else if (transform.position.y > 30 && transform.position.y < 80)
                transform.Translate(0, 1.6f * FireSpeed * Time.deltaTime, 0);
            else
                transform.Translate(0, FireSpeed * Time.deltaTime, 0);
        }
    }
}