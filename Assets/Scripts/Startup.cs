using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void InstantiatePrefabs()
	{
		Debug.Log("-- Intantiating objects --");

		GameObject[] prefabToInstantiate = Resources.LoadAll<GameObject>("InstantiateOnLoad/");
		foreach(GameObject prefab in prefabToInstantiate)
		{
			Debug.Log($"Creating {prefab.name}");
			GameObject.Instantiate(prefab);
		}

		Debug.Log("-- Intantiating objects done --");

	}
}
