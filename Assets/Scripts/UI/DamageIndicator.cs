using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
	private Coroutine coroutine;
	public Image image;
	public float flashSpeed;

	private void Start()
	{
		CharacterManager.Instance.Player.Condition.onTakeDamage += Flash;
	}

	public void Flash()
	{
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		image.enabled = true;
		image.color = new Color(1f, 100f / 255f, 100f / 255f, 90f / 255f);
		coroutine = StartCoroutine(FadeAway());
	}

	IEnumerator FadeAway()
	{
		float startAlpha = 90f / 255f;
		float a = startAlpha;

		while(a > 0)
		{
			a -= (startAlpha / flashSpeed) * Time.deltaTime;
			image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
			yield return null;
		}

		image.enabled = false;
	}
}
