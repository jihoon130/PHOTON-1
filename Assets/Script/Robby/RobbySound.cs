using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbySound : MonoBehaviour
{
    AudioSource Audio;
    public AudioClip[] audios;
    public AudioSource mainaudio;
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        if (mainaudio &&PlayerPrefs.HasKey("hyogwa"))
            mainaudio.volume = PlayerPrefs.GetFloat("hyogwa");
        if (Audio && PlayerPrefs.HasKey("baegung"))
            Audio.volume = PlayerPrefs.GetFloat("baegung");
    }

    public void SoundPlayer(int type)
    {
        Audio.clip = audios[type];
        Audio.Play();
    }
}
