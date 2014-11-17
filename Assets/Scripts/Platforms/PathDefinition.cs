using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathDefinition : MonoBehaviour
{
    public Transform[] Points;

    public IEnumerator<Transform> GetPathsEnumerator()
    {
        if (Points == null || Points.Length < 1)
            yield break;            // Will terminate the sequence

        var direction = 1;
        var index = 0;
        while (true)
        {
            yield return Points[index]; // Yields/returns the execution to the one invoking the enumerator

            if (Points.Length == 1)
                continue;

            if (index <= 0)
                direction = 1;
            else if (index >= Points.Length - 1)
                direction = -1;

            index = index + direction;
        }
    }

    public void OnDrawGizmos()
    {
        if (Points == null || Points.Length < 2)    // Ensure that we have enough points to create a line
            return;

        var points = Points.Where(t => t != null).ToList(); // Turn to a list

        if (points.Count < 2)
            return;

        for (int i = 1; i < points.Count; i++)     // Loop through all the points
        {
            Gizmos.DrawLine(points[i - 1].position, points[i].position);    // Drawing from the previous point to the next
        }
    }
}
