using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class CarLapCounter : MonoBehaviour
{ 
	public TMP_Text carPositionText;

	int passCheckpointNumber = 0;
    float timeAtLastPassedCheckpoint = 0;

	int numberOfPassedCheckpoint = 0;

	int lapCompleted = 0;
	const int lapsToComplete = 2;

	bool isRaceCompleted = false;

	int carPosition = 0;
	
	//Events
	public event Action<CarLapCounter> OnPassCheckpoint;

	public void SetCarPositon(int position)
	{
		carPosition = position;
	}

	public int GetNumberOfPassedCheckpoint()
	{
		return numberOfPassedCheckpoint;
	}

	public float GetTimeAtLastPassedCheckpoint()
	{
		return timeAtLastPassedCheckpoint;
	}

	IEnumerator ShowPositionCO(float delayUntilHidePosition)
	{
		Debug.Log(carPosition);
		carPositionText.text = carPosition.ToString();
		Debug.Log("carPositionText.text: " + carPositionText.text);

		carPositionText.gameObject.SetActive(true);

		yield return new WaitForSeconds(delayUntilHidePosition);

		carPositionText.gameObject.SetActive(false);

	}
	void OnTriggerEnter2D(Collider2D collider2D)
	{
		if(collider2D.CompareTag("Checkpoint"))
		{
			if(isRaceCompleted)
			{
				return;
			}
			Checkpoint checkpoint = collider2D.GetComponent<Checkpoint>();

			//make sure the car is passing the checkpoint at right order
			if (passCheckpointNumber + 1 == checkpoint.checkPointNumber)
			{
				passCheckpointNumber = checkpoint.checkPointNumber;

				numberOfPassedCheckpoint++;

				timeAtLastPassedCheckpoint = Time.time;

				if(checkpoint.isFinishLine)
				{
					passCheckpointNumber = 0;
					lapCompleted++;
					if (lapCompleted >= lapsToComplete)
					{
						isRaceCompleted = true;
					}
				}

				OnPassCheckpoint?.Invoke(this);

				if(isRaceCompleted)
				{
					StartCoroutine(ShowPositionCO(100));

					if (CompareTag("Player"))
					{
						GameManager.instance.OnGameCompleted();
						GetComponent<CarInputHandler>().enabled = false;
						GetComponent<AICarHandler>().enabled = true;
					}
				} else if(checkpoint.isFinishLine) 
				{
					StartCoroutine(ShowPositionCO(1.5f));
				}
			}
		}
	}
}
