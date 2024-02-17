using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
	//CAR SETTING
	public float driftFactor = 0.95f;
	public float accelerationFactor = 30.0f;
	public float turnFactor = 3.5f;
	public float maxSpeed = 20;

	//SPRITES
	public SpriteRenderer carSpriteRenderer;
	public SpriteRenderer carShadowRenderer;

	//ANIMATION CURVE
	public AnimationCurve jumpCurve;

	//Local variables
	float accelerationInput = 0;
	float steeringInput = 0;

	float rotationAngle = 0;

	float velocityVsUp = 0;

	bool isJumping = false;

	//Components
	Rigidbody2D carRigidbody2D;
	Collider2D carCollider;

	//Awake is called when the script instance is being loaded
	void Awake()
	{
		carRigidbody2D = GetComponent<Rigidbody2D>();
		carCollider = GetComponentInChildren<Collider2D>();
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
		velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);
	
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
		if(carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0 && !isJumping)
		{
			return;
		}

		//Apply drag if there is no accelerationInput so the car stops when the player lets go off the accelerator
		if (accelerationInput == 0)
		{
			carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
		} else
		{
			carRigidbody2D.drag = 0;
		}
		//Create a force for the engine
		Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

		//Apply force and pushes the car forward
		carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
	}

	void ApplySteering()
	{
		//Limit the car ability to turn when moving slowly
		float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);
		minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
		//Update rotation angle based on input 
		rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

		//Apply steering by rotating the car object
		carRigidbody2D.MoveRotation(rotationAngle);
	}

	void killOrthogonalVelocity()
	{
		Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
		Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

		carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;

	}

	float GetLateralVelocity()
	{
		//return how fast the car is going sideways
		return Vector2.Dot(transform.right, carRigidbody2D.velocity);
	}

	public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
	{
		lateralVelocity = GetLateralVelocity();
		isBraking = false;
		if(isJumping)
		{
			return false;
		}

		//check if car is moving forward and if the player is hitting the brake. 
		if(accelerationInput < 0 && velocityVsUp > 0)
		{
			isBraking = true;
			return true;
		}
		//check if car have a lot of side movement
		if(Mathf.Abs(GetLateralVelocity()) > 4.0f)
		{
			return true;
		}

		return false;
	}

	public float GetVelocityMagnitude()
	{
		return carRigidbody2D.velocity.magnitude;
	}

	public void SetInputVector(Vector2 inputVector)
	{
		steeringInput  = inputVector.x;
		accelerationInput = inputVector.y;
	}

	public void Jump(float jumpHeightScale, float jumpPushScale)
	{
		if(!isJumping)
		{
			StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale));
		}
	}


	private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
	{
		isJumping = true;

		float jumpStartTime = Time.time;
		float jumpDuration = carRigidbody2D.velocity.magnitude * 0.015f;

		jumpHeightScale = jumpHeightScale * carRigidbody2D.velocity.magnitude * 0.05f;
		jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.0f, 1.0f);
		//Dissable collision
		carCollider.enabled = false;

		//Push the object forward when passed a jump
		carRigidbody2D.AddForce(carRigidbody2D.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

		while(isJumping)
		{

			//Percentage 0 - 1 of where we are in the jumping process
			float jumpCompletePercentage = (Time.time - jumpStartTime) / jumpDuration;
			jumpCompletePercentage = Mathf.Clamp01(jumpCompletePercentage);

			//take the base scale of 1 and add how much we should increase the scale
			carSpriteRenderer.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletePercentage)* jumpHeightScale;

			carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale * 0.75f;
			carShadowRenderer.sortingOrder = carSpriteRenderer.sortingOrder-1;
			Debug.Log("Car: "+ carSpriteRenderer.transform.localScale+ ", "+ carSpriteRenderer.transform.localPosition);
			//Debug.Log("Shadow: " + carShadowRenderer.transform.localScale + ", " + carShadowRenderer.transform.localPosition);
			Debug.Log("Jump curve: " + jumpCurve.Evaluate(jumpCompletePercentage));

			carShadowRenderer.transform.localPosition = new Vector3(1,-1,0.0f) * 9 * jumpCurve.Evaluate(jumpCompletePercentage) * jumpHeightScale;
			if (jumpCompletePercentage == 1.0f) break;

			yield return null;
		}

		if (Physics2D.OverlapCircle(transform.position, 1.5f))
		{
			//jump again if there are something below the car
			isJumping = false;

			//add a small jump and push the car forward a bit
			Jump(0.2f, 0.6f);
		}
		else
		{
			//handle landing, scale back the object
			carSpriteRenderer.transform.localScale = Vector3.one;

			carShadowRenderer.transform.localPosition = Vector3.zero;
			carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale;

			//Enable collision
			carCollider.enabled = true;
			carShadowRenderer.sortingOrder = -2;

			isJumping = false;
		}



	}

	//Detect jump trigger
	void OnTriggerEnter2D(Collider2D collider2D)
	{
		if(collider2D.CompareTag("Jump"))
		{
			Debug.Log("JUMPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPp");
			//Get the jump data from the jump
			JumpData jumpData = collider2D.GetComponent<JumpData>();
			Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
		}
	}
}
