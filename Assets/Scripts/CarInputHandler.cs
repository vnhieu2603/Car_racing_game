using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
	public int playerNumber = 1;

    TopDownCarController topDownCarController;

	//Awake is called when the script instance is being loaded
	void Awake()
	{
		topDownCarController = GetComponent<TopDownCarController>();
	}


    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
  //      inputVector.x = Input.GetAxis("Horizontal");
		//inputVector.y = Input.GetAxis("Vertical");

		switch(playerNumber)
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

		topDownCarController.SetInputVector(inputVector);

	}
}
