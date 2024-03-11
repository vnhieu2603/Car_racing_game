using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
	[Header("Max speed when passing this waypoint")]
	public float maxSpeed = 0;

	[Header("This is the waypoint car going to reach, not yet")]
	public float minDistanceToNextWaypoint = 10;

	public WaypointNode[] nextWayPointNode;
}
