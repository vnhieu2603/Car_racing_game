using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandle : MonoBehaviour
{
    //LeaderboardUIHandler leaderboardUIHandler;

	public List<CarLapCounter> carLapCounterList = new List<CarLapCounter>();
    // Start is called before the first frame update
    void Awake()
    {


    }

    void Start()
	{
        //leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

        //get all CarLapCounter in Scene
        CarLapCounter[] carLapCountersArray = FindObjectsOfType<CarLapCounter>();
		//Debug.Log("car lap array: " + carLapCountersArray[0] + ", " + carLapCountersArray[1] + ", " + carLapCountersArray[2] + ", " + carLapCountersArray[3]);
		//store the lap counters in list
		carLapCounterList = carLapCountersArray.ToList<CarLapCounter>();

		foreach (CarLapCounter lapCounters in carLapCounterList)
		{
			lapCounters.OnPassCheckpoint += OnPassCheckPoint;
		}
		//leaderboardUIHandler.UpdateList(carLapCounterList);	
	}

	void OnPassCheckPoint(CarLapCounter carLapCounter)
    {
		Debug.Log("car lap counter: " + carLapCounter);
		//Debug.Log($"Event: car " + carLapCounter.gameObject.name + " passed a checkpoint");
		carLapCounterList = carLapCounterList.OrderByDescending(c => c.GetNumberOfPassedCheckpoint()).ThenBy(c => c.GetTimeAtLastPassedCheckpoint()).ToList();

        int carPosition = carLapCounterList.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPositon(carPosition);

        //leaderboardUIHandler.UpdateList(carLapCounterList);


    }

}
