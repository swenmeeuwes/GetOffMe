using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public List<AudioClip> musicChannels;
    public AudioClip menuMusic;

    private AudioSource[] sources;

    //I could've just used the array with indices, but this is easier when you wan't to mute/unmute stuff
    private AudioSource bass;
    private AudioSource noise;
    private AudioSource lead;
    private AudioSource lead2;

    private AudioSource menu;

    // Use this for initialization
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        sources = GetComponents<AudioSource>();

        bass = sources[0];
        noise = sources[1];
        lead = sources[2];
        lead2 = sources[3];

        menu = sources[4];

        bass.clip = musicChannels[0];
        noise.clip = musicChannels[1];
        lead.clip = musicChannels[2];
        lead2.clip = musicChannels[3];

        menu.clip = menuMusic;

        sources[4].clip = menuMusic;
    }
    void Start() {
        PlayMenu();
    }
    public void PlayStage() {
        menu.Stop();

        bass.Play();
        noise.Play();
        lead.Play();
        lead2.Play();

        lead.mute = true;
        lead2.mute = true;
    }
    public void PlayMenu()
    {
        bass.Stop();
        noise.Stop();
        lead.Stop();
        lead2.Stop();

        menu.Play();

    }
}
