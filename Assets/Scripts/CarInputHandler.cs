using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
	public int playerNumber = 1;
	Vector2 inputVector = Vector2.zero;
	public bool isUIInput = false;
    TopDownCarController topDownCarController;

	//Awake is called when the script instance is being loaded
	void Awake()
	{
		topDownCarController = GetComponent<TopDownCarController>();
	}


    // Update is called once per frame
    void Update()
    {
  //      inputVector.x = Input.GetAxis("Horizontal");
		//inputVector.y = Input.GetAxis("Vertical");
		if(isUIInput )
		{

		} else
		{
			inputVector = Vector2.zero;
			switch (playerNumber)
			{
				case 1:
					inputVector.x = Input.GetAxis("Horizontal_P1");
					inputVector.y = Input.GetAxis("Vertical_P1");
					break;
				case 2:
					inputVector.x = Input.GetAxis("Horizontal_P2");
					inputVector.y = Input.GetAxis("Vertical_P2");
					break;
				case 3:
					inputVector.x = Input.GetAxis("Horizontal_P3");
					inputVector.y = Input.GetAxis("Vertical_P3");
					break;
				case 4:
					inputVector.x = Input.GetAxis("Horizontal_P4");
					inputVector.y = Input.GetAxis("Vertical_P4");
					break;

			}
		}
		

		topDownCarController.SetInputVector(inputVector);

	}

	public void SetInput(Vector2 newInput)
	{
		inputVector = newInput;
	}
	

	public NetworkInputData GetNetworkInput()
	{
		NetworkInputData networkInputData = new NetworkInputData();
		networkInputData.direction = inputVector;

		return networkInputData;
	}
}
