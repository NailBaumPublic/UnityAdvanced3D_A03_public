using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class WeatherController : MonoBehaviour
{
    public float coldValue = 0;
    public ParticleSystem particleSystem;

    public void Update()
    {
        CharacterManager.Instance.Player.Condition.Heated(coldValue * Time.deltaTime);
    }

}
