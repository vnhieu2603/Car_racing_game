using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
	public float driftFactor = 0.95f;
	public float accelerationFactor = 30.0f;
	public float turnFactor = 3.5f;
	public float maxSpeed = 20;
	//Local variables
	float accelerationInput = 0;
	float steeringInput = 0;

	float rotationAngle = 0;

	float velocityVsUp = 0;
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

		killOrthogonalVelocity();

		ApplySteering();
	}
	
	void ApplyEngineForce()
	{
		//Calculate how much "forward" we are going in terms of the direction of our velocity
		velocityVsUp = Vector2.Dot(transform.up, carRigidbody2.velocity);
		if(accelerationInput != 0)
		{
			Debug.Log("Acce: "+ accelerationInput);

		}
		if(steeringInput != 0)
		{
			Debug.Log("Steering: " + steeringInput);

		}
		//Limit so we cannot go faster than max speed in the "forward" direction
		if (velocityVsUp > maxSpeed && accelerationInput > 0)
		{
			return;
		}
		//Limit so we cannot go faster than 50% of max speed in the "reverse" direction
		if(velocityVsUp < -maxSpeed*0.5f && accelerationInput < 0)
		{
			return;
		}
		//Limit so we cannot go faster in any direction while accelerating
		if(carRigidbody2.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
		{
			return;
		}

		//Apply drag if there is no accelerationInput so the car stops when the player lets go off the accelerator
		if (accelerationInput == 0)
		{
			carRigidbody2.drag = Mathf.Lerp(carRigidbody2.drag, 3.0f, Time.fixedDeltaTime * 3);
		} else
		{
			carRigidbody2.drag = 0;
		}
		//Create a force for the engine
		Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

		//Apply force and pushes the car forward
		carRigidbody2.AddForce(engineForceVector, ForceMode2D.Force);
	}

	void ApplySteering()
	{
		//Limit the car ability to turn when moving slowly
		float minSpeedBeforeAllowTurningFactor = (carRigidbody2.velocity.magnitude / 8);
		minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
		//Update rotation angle based on input 
		rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

		//Apply steering by rotating the car object
		carRigidbody2.MoveRotation(rotationAngle);
	}

	void killOrthogonalVelocity()
	{
		Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2.velocity, transform.up);
		Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2.velocity, transform.right);

		carRigidbody2.velocity = forwardVelocity + rightVelocity * driftFactor;

	}

	public void SetInputVector(Vector2 inputVector)
	{
		steeringInput  = inputVector.x;
		accelerationInput = inputVector.y;
	}
}
