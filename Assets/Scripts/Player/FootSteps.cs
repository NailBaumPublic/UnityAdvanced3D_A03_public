using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
	public AudioClip[] footstepClips;
	private AudioSource audioSource;
	private Rigidbody _rigidbody;
	public float footstepThreshold;
	public float footstepRate;
	private float footStepTime;


}
