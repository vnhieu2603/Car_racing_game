using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderBoard[] setLeaderBoard;

	Canvas canvas;

	void Awake()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;

		GameManager.instance.OnGameStateChanged += OnGameStateChanged;
	}
	// Start is called before the first frame update
	void Start()
    {
		VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

		CarLapCounter[] carLapCountersArray = FindObjectsOfType<CarLapCounter>();

		setLeaderBoard = new SetLeaderBoard[carLapCountersArray.Length];

		for (int i = 0; i < carLapCountersArray.Length; i++)
		{
			GameObject leaderboardGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

			setLeaderBoard[i] = leaderboardGameObject.GetComponent<SetLeaderBoard>();

			setLeaderBoard[i].SetPositionText($"{i + 1}.");
		}
	}
    
    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        for(int i =0; i < lapCounters.Count;i++)
        {
            setLeaderBoard[i].SetDriverNameText(lapCounters[i].gameObject.name);
        }
    }

	void OnGameStateChanged(GameManager gameManager)
	{
		if(GameManager.instance.GetGameState() == GameState.raceOver)
		{
			canvas.enabled = true;
		}
	}

     void OnDestroy()
    {
		GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
