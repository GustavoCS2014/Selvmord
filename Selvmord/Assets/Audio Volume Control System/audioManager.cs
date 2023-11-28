using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public static audioManager Instance;
    private AudioSource audioSource;
    private AudioClip LastAudio;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void ReproduceSound(AudioClip sound)
    {
        LastAudio = audioSource.isPlaying ? sound : null;

        if (LastAudio == sound) return;
        LastAudio = sound;
        audioSource.PlayOneShot(sound);
    }
}
