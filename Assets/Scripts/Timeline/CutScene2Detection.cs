using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene2Detection : MonoBehaviour
{
	public CutsceneManager CutsceneManager;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			CutsceneManager.StartNewCutScene(1);
			Destroy(gameObject);
		}
	}
}
