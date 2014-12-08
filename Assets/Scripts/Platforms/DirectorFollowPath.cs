using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectorFollowPath : MonoBehaviour
{
    public enum FollowType
    {
        MoveTowards,
		Lerp
	}

    public FollowType Type = FollowType.MoveTowards;
    public PathDefinition Path; // Reference to PathDefinition so that we know whick path to follow
    public float Speed = 1;
    public float MaxDistanceToGoal = 0.1f;

    private IEnumerator<Transform> _currentPoint;
	private bool _isFacingRight;
	private bool MakeSwoop;		

	public void Awake()
	{
		_isFacingRight = transform.localScale.x > 0;       // Sets the initial value of the bool depending on where the character stands in a new level
		MakeSwoop = true;
	}

    public void Start()
    {
        if (Path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        _currentPoint = Path.GetPathsEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;

        transform.position = _currentPoint.Current.position;
    }

    public void Update()
    {

		if (MakeSwoop)
		{
			if (_currentPoint == null || _currentPoint.Current == null)
				return;
			if (Type == FollowType.MoveTowards)
				transform.position = Vector3.MoveTowards (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
			else if (Type == FollowType.Lerp)
				transform.position = Vector3.Lerp (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
			var distanceSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;   // sqrMagnitude is faster than the sqroot
			if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
				_currentPoint.MoveNext ();
		}

		if (_currentPoint.Current.position.x > transform.position.x)
		{
			if (!_isFacingRight)
				Flip();
		}
		else if (_currentPoint.Current.position.x < transform.position.x)
		{
			if (_isFacingRight)
				Flip();
		}
    }

	private void Flip()
	{
		_isFacingRight = !_isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
