using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
	private static ResourceSpawner _instance;
	public static ResourceSpawner Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject("ResourceSpawner").AddComponent<ResourceSpawner>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			if (_instance != this)
			{
				Destroy(gameObject);
			}
		}
	}

	public void RespawnInSeconds(GameObject resource)
	{
		StartCoroutine(RespawnResource(resource));
	}

	public IEnumerator RespawnResource(GameObject resource)
	{
		yield return new WaitForSeconds(5f);

		resource.SetActive(true);
		resource.GetComponent<Resource>().RestoreHealth();
	}
}
