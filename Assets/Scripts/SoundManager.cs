using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    private AudioSource audiosource;
    public AudioClip DungeonClip;
    public AudioClip MutantSong;
    public AudioClip MutantTalkClip;
    public AudioClip NatureClip;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_instance != null)
            {
                Destroy(gameObject);
            }
        }
        audiosource = GetComponent<AudioSource>();
    }

    public void BackgroundMusicMute()
    {
        audiosource.Stop();
    }

    public void MutantMusicOn()
    {
        audiosource.clip = MutantSong;
        audiosource.Play();
    }
    public void DungeonMusicOn() 
    {
        audiosource.clip = DungeonClip;
        audiosource.Play();
    }
    public void MutantTalkMusicOn()
    {
        audiosource.clip = MutantTalkClip;
        audiosource.Play();
    }

    public void NatureMusicOn()
    {
        audiosource.clip= NatureClip;
        audiosource.Play();
    }
}
