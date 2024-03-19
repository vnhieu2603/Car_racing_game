using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log(spawnPoints.Length);
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for(int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            int playerSelectedCarID = PlayerPrefs.GetInt($"P{i+1}SelectedCarID");
            Debug.Log("playerSelectedCarID: " + playerSelectedCarID);
            //Find the player cars prefabs
            foreach( CarData carData in carDatas )
            {
                //We found the car data for the player
                if(carData.CarUniqueID == playerSelectedCarID )
                {
                    //Spawn it on the spawn point
                    GameObject playerCar = Instantiate(carData.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    playerCar.GetComponent<CarInputHandler>().playerNumber = i + 1;
                    break;
                }
            }
        }
    }
}
    
