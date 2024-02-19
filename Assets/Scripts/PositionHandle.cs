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
		//get all CarLapCounter in Scene
		CarLapCounter[] carLapCountersArray = FindObjectsOfType<CarLapCounter>();

		//store the lap counters in list
		carLapCounterList = carLapCountersArray.ToList<CarLapCounter>();

        foreach(CarLapCounter lapCounters in carLapCounterList)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckPoint;
        }

        //leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
	}

	void Start()
	{
		//leaderboardUIHandler.UpdateList(carLapCounterList);	
	}

	void OnPassCheckPoint(CarLapCounter carLapCounter)
    {
		//Debug.Log($"Event: car " + carLapCounter.gameObject.name + " passed a checkpoint");
		carLapCounterList = carLapCounterList.OrderByDescending(c => c.GetNumberOfPassedCheckpoint()).ThenBy(c => c.GetTimeAtLastPassedCheckpoint()).ToList();

        int carPosition = carLapCounterList.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPositon(carPosition);

		//leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

	}

}
