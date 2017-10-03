using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour {

    // Use this for initialization
    public static SoundManager Instance;

    public List<AudioMap> SFX;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
            Debug.LogWarning("Another SoundManager was already instantiated!");

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
	void Start () {
		
	}
    public void PlaySound(SFXType type) {
        if (PlayerPrefs.GetInt(PlayerPrefsLiterals.MUTE_SFX.ToString(), 0) == 1)
            return;

        var clip = SFX.Where((sfx) => sfx.type == type).First().clip;
        if (clip){
            audioSource.PlayOneShot(clip);
        }
        else {
            Debug.LogWarning("Trying to play a sound that does not exist!");
        }
    }
}
