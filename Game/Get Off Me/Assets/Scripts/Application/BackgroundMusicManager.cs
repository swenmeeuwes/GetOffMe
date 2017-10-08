using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour {

    public List<AudioClip> musicChannels;

    private AudioSource[] sources;

    public static BackgroundMusicManager Instance;

    //I could've just used the array with indices, but this is easier when you wan't to mute/unmute stuff
    private AudioSource bass;
    private AudioSource noise;
    private AudioSource lead;
    private AudioSource lead2;


    private int playingComboTier = 0;

    // Use this for initialization
    void Awake() {
        if (Instance != null)
            Debug.LogWarning("Another BackgroundMusicManager was already instantiated!");

        Instance = this;

        sources = GetComponents<AudioSource>();

        bass = sources[0];
        noise = sources[1];
        lead = sources[2];
        lead2 = sources[3];

        bass.clip = musicChannels[0];
        noise.clip = musicChannels[1];
        lead.clip = musicChannels[2];
        lead2.clip = musicChannels[3];
    }
    void Start() {
        if (PlayerPrefs.GetInt(PlayerPrefsLiterals.MUSIC.ToString(), 1) == 1)
            PlayStage();
    }
    public void PlayStage() {

        bass.Play();
        noise.Play();
        lead.Play();
        lead2.Play();

        Mute(lead);
        Mute(lead2);
    }
    public void HandleComboTier(int tier) {
        if (tier == playingComboTier) { return; }

        Mute(bass);
        Mute(noise);
        Mute(lead);
        Mute(lead2);
        SetPitch(1.0f);

        if (tier >= 0)
        {
            Play(bass);
            Play(noise);
        }
        if (tier >= 2)
        {
            Play(lead);
        }
        if (tier >= 5)
        {
            Play(lead2);
        }
        if (tier >= 9) {
            SetPitch(1.1f);
        }
        playingComboTier = tier;
    }
    private void Mute(AudioSource source) {
        source.mute = true;
    }
    private void Play(AudioSource source) {
        source.mute = false;
    }
    private void SetPitch(float pitch) {
        bass.pitch = pitch;
        noise.pitch = pitch;
        lead.pitch = pitch;
        lead2.pitch = pitch;
    }
}
