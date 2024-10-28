using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class PatrolNetwork : MonoBehaviour
{
    [Tooltip("Stored list of transforms for patrol waypoints. If not manually set, this will auto-construct based on " +
        "children of this gameobject.")]
    public List<Transform> waypoints;

    [Header("Gizmo Values")]
    [SerializeField] private Color _gizmoColor;
    [SerializeField] private float _gizmoRadius;

    private void OnEnable()
    {
        if(waypoints.Count() > 0)
        {
            Debug.Log("Waypoint list already set!");
            return;
        }

        Transform[] foundWaypoints = GetComponentsInChildren<Transform>();      // Grab all transforms from children.
        Debug.Log("Found" + foundWaypoints.Count() + " transforms...");

        for (int i = 0; i < foundWaypoints.Count(); i++)
        {
            if (foundWaypoints[i] == transform)
                continue;
            else
                waypoints.Add(foundWaypoints[i].transform);               // Add each child transform to the list.
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;

        // Iterate through stored list of waypoints.
        for (int i = 0; i < waypoints.Count(); i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, _gizmoRadius);                     // Draw sphere at waypoint location.

            if (i + 1 < waypoints.Count())
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);      // Draw line to next waypoint.
            else
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);          // Draw line back to first waypoint.
        }
    }
}
