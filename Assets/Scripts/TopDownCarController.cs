using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
	public float accelerationFactor = 30.0f;
	public float turnFactor = 3.5f;

	//Local variables
	float accelerationInput = 0;
	float steeringInput = 0;
	float rotationAngle = 0;

	//Components
	Rigidbody2D carRigidbody2;

	//Awake is called when the script instance is being loaded
	void Awake()
	{
		carRigidbody2 = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		ApplyEngineForce();

		ApplySteering();
	}
	
	void ApplyEngineForce()
	{
		//Create a force for the engine
		Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

		//Apply force and pushes the car forward
		carRigidbody2.AddForce(engineForceVector, ForceMode2D.Force);
	}

	void ApplySteering()
	{
		//Update rotation andle based on input 
		rotationAngle -= steeringInput * turnFactor;

		//Apply steering by rotating the car object
		carRigidbody2.MoveRotation(rotationAngle);
	}

	public void SetInputVector(Vector2 inputVector)
	{
		steeringInput  = inputVector.x;
		accelerationInput = inputVector.y;
	}
}
