using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICarHandler : MonoBehaviour
{

    public enum AIMode { followPlayer, followWayPoints};

    public AIMode aiMode;
    public float maxSpeed = 20;

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    //waypoint
    WaypointNode currentWayPoint = null;
    WaypointNode[] allWayPoints;

    PolygonCollider2D polygonCollider2D;

    TopDownCarController topDownCarController;

	void Awake()
	{
        topDownCarController = GetComponent<TopDownCarController>();
        allWayPoints = FindObjectsOfType<WaypointNode>();

        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        switch(aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;
            case AIMode.followWayPoints:
                FollowWaypoints();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyBrake(inputVector.x);

        topDownCarController.SetInputVector(inputVector);

	}

    void FollowWaypoints()
    {
        if(currentWayPoint == null)
        {
            currentWayPoint = FindClosestWaypoint();
        } else
        {
            targetPosition = currentWayPoint.transform.position;

            float distanceToWayPoint = (targetPosition - transform.position).magnitude;
            //check if we are close enough to reach the waypoint
            if(distanceToWayPoint <= currentWayPoint.minDistanceToNextWaypoint)
            {
                if(currentWayPoint.maxSpeed > 0)
                {
                    maxSpeed = currentWayPoint.maxSpeed;
                } 
                //if we are close enough then go to the next way point
                currentWayPoint = currentWayPoint.nextWayPointNode[0];
            }
        }
    }

    WaypointNode FindClosestWaypoint()
    {
        return allWayPoints.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    }

	void FollowPlayer()
    {
        if(targetTransform == null)
        {
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
            
        }

        if(targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }
    }
        
    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        //Debug.Log("vector to target: "+ vectorToTarget);
        vectorToTarget.Normalize();

        //calculate an angle towards the target
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
  
        //Debug.Log("angel to target: " +  angleToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    float ApplyBrake(float inputX)
    {
        if(topDownCarController.GetVelocityMagnitude() > maxSpeed)
        {
            return 0;
        }
        
        return 1.05f - Mathf.Abs(inputX);
    }


    
}
