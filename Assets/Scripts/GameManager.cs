using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { countDown, running, raceOver};
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

	GameState gameState = GameState.countDown;

	//Events
	public event Action<GameManager> OnGameStateChanged;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		} else if( instance != this )
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

	void LevelStart()
	{
		gameState = GameState.countDown;

		Debug.Log("Level Started");
	}
	
	public GameState GetGameState()
	{
		return gameState;
	}

	void ChangeGameSate(GameState newGameState)
	{
		if(gameState != newGameState)
		{
			gameState = newGameState;

			//Invoke game state change event
			OnGameStateChanged?.Invoke(this);
		}
	}
	public void OnRaceStart()
	{
		Debug.Log("On race start");
		ChangeGameSate(GameState.running);
	}

	public void OnGameCompleted()
	{
		Debug.Log("On race completed");
		ChangeGameSate(GameState.raceOver);
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		LevelStart();
	} 
}
