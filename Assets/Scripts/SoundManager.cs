using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSoucre;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    // Use this for initialization
    void Awake () {

        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy (gameObject);

        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle(AudioClip clip)
    {
        efxSoucre.clip = clip;
        efxSoucre.Play();
    }

    public void RandomizesSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSoucre.pitch = randomPitch;
        efxSoucre.clip = clips[randomIndex];
        efxSoucre.Play();
    }
 }
