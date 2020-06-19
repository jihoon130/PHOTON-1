using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbySound : MonoBehaviour
{
    AudioSource Audio;
    public AudioClip[] audios;

    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    public void SoundPlayer(int type)
    {
        Audio.clip = audios[type];
        Audio.Play();
    }
}
