using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarHandler : MonoBehaviour
{

    public enum AIMode { followPlayer, followWayPoints};

    public AIMode aiMode;
    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    TopDownCarController topDownCarController;

	void Awake()
	{
        topDownCarController = GetComponent<TopDownCarController>();	
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
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = 1.0f;

        topDownCarController.SetInputVector(inputVector);

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
        Debug.Log("vector to target: "+ vectorToTarget);
        vectorToTarget.Normalize();

        //calculate an angle towards the target
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
  
        Debug.Log("angel to target: " +  angleToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }
}
