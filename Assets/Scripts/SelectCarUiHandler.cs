using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectCarUiHandler : MonoBehaviour
{
    public TMP_Text mapText;

	[Header("Car prefab")]
    public GameObject carPrefab;

    [Header("Spawn on")]
    public Transform spawnOnTransform;

    bool isChangingCar = false;

    CarData[] carDatas;

    int selectedCarIndex = 0;
    int selectedMapIndex = 1;
    int totalMap = 0;
    string selectedMap;

	CarUIHandler carUIHandler = null;

    // Start is called before the first frame update
    void Start()
    {
        totalMap = SceneManager.sceneCountInBuildSettings;

		carDatas = Resources.LoadAll<CarData>("CarData/");
        Debug.Log("Total scene: " + totalMap);
		
		StartCoroutine(SpawnCarCO(true));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            OnPreviousCar();
		} else if (Input.GetKey(KeyCode.RightArrow))
        {
            OnNextCar();
		}

        if(Input.GetKey(KeyCode.Space)) {
            OnSelectCar();
        }
	}

    public void OnPreviousCar()
    {
        if(isChangingCar)
        {
            return;
        }
		selectedCarIndex--;
		if (selectedCarIndex < 0)
		{
			selectedCarIndex = carDatas.Length - 1;
		}
		StartCoroutine(SpawnCarCO(true));
	}
	public void OnNextCar()
	{
		if (isChangingCar)
		{
			return;
		}
		selectedCarIndex++;
        if(selectedCarIndex > carDatas.Length - 1)
        {
            selectedCarIndex = 0;
        }
		StartCoroutine(SpawnCarCO(false));
	}

    public void OnPreviousMap()
    {
		selectedMapIndex--;
        if(selectedMapIndex < 1)
        {
            selectedMapIndex = totalMap - 1;
		}
		string path = SceneUtility.GetScenePathByBuildIndex(selectedMapIndex);
		int slash = path.LastIndexOf('/');
		string name = path.Substring(slash + 1);
		int dot = name.LastIndexOf('.');
		Debug.Log("Scene name: " + name.Substring(0, dot));
		mapText.text = name.Substring(0, dot);
        selectedMap = name.Substring(0, dot);

	}
	public void OnNextMap()
	{
		selectedMapIndex++;
		if (selectedMapIndex > totalMap - 1)
		{
			selectedMapIndex = 1;
		}
		string path = SceneUtility.GetScenePathByBuildIndex(selectedMapIndex);
		int slash = path.LastIndexOf('/');
		string name = path.Substring(slash + 1);
		int dot = name.LastIndexOf('.');
		Debug.Log("Scene name: " + name.Substring(0, dot));
		mapText.text = name.Substring(0, dot);
		selectedMap = name.Substring(0, dot);

	}
	public void OnSelectCar()
    {
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
		PlayerPrefs.SetInt("P2SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
		PlayerPrefs.SetInt("P3SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
		PlayerPrefs.SetInt("P4SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);

        PlayerPrefs.Save();

        SceneManager.LoadScene(selectedMap);
	}
	IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isChangingCar = true;

        if(carUIHandler != null)
        {
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);
        }

        GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);

        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.SetupCar(carDatas[selectedCarIndex]);
        carUIHandler.StartCarEntranceAnimation(isCarAppearingOnRightSide);

        yield return new WaitForSeconds(0.4f);

        isChangingCar = false;
    }
}
