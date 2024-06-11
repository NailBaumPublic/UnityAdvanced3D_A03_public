using System;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
	float time;
	[Range(0f, 1f)]
	public float TimeRate;

	public Light Glow;
	public AnimationCurve GlowIntensity;

	private void Start()
	{
		time = 0f;
	}

	private void Update()
	{
		time = (time + TimeRate * Time.deltaTime);
		if (time > 1.0f)
		{
			time = 0.0f;
		}

		UpdateLighting();
	}

	private void UpdateLighting()
	{
		Glow.intensity = GlowIntensity.Evaluate(time) * 40f;
	}
}
