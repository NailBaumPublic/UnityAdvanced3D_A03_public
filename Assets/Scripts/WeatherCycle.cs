using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum weatherState
{
    SUN,
    RAIN,
    SNOW
}

public class WeatherCycle : MonoBehaviour
{
    public GameObject rain;
    public GameObject snow;
    public float durationTime;

    public int RainColdValue;
    public int SnowColdValue;

    private float nowtime = 0;
    private int weathernum = 0;
    private ParticleSystem weatherParticleSystem = new ParticleSystem();
    private void Start()
    {
        WeatherDisable(rain);
        WeatherDisable(snow);
    }

    void Update()
    {
        nowtime += Time.deltaTime;

        if(nowtime > durationTime)
        {
            weathernum = Random.Range(0, 2);

            DuringWeather(weathernum);

            nowtime = 0;
        }
    }

    private void DuringWeather(int weathernum)
    {
        switch (weathernum)
        {
            case (int)weatherState.SUN:
                WeatherDisable(rain);
                WeatherDisable(snow);
                break;
            case (int)weatherState.RAIN:
                WeatherEnable(rain,RainColdValue);
                WeatherDisable(snow);
                break;
            case (int)weatherState.SNOW:
                WeatherEnable(snow,SnowColdValue);
                WeatherDisable(rain);
                break;
        }

    }

    private void WeatherEnable(GameObject Weather,int Value)
    {
        weatherParticleSystem = Weather.GetComponent<ParticleSystem>();
        weatherParticleSystem.Play();
        Weather.GetComponent<WeatherController>().coldValue = Value;
    }

    private void WeatherDisable(GameObject Weather)
    {
        weatherParticleSystem = Weather.GetComponent<ParticleSystem>();
        weatherParticleSystem.Stop();
        Weather.GetComponent<WeatherController>().coldValue = 0;
    }

}
