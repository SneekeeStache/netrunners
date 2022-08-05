using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public List<Transform> waypoints;

    private void Awake()
    {
        FetchWaypoints();
    }

    private void FetchWaypoints()
    {
        waypoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints.Count == 0 || waypoints.Count != transform.childCount)
        {
            FetchWaypoints();
        }

        Vector3 lastPosition = waypoints[waypoints.Count - 1].position;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawLine(lastPosition, waypoint.position);
            lastPosition = waypoint.position;
        }
    }
}
