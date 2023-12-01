using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    private AudioClip LastAudio;
    [SerializeField] private AudioClip DamageSound;
    [SerializeField] private AudioClip DeathSound;

    

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
        /* MenusControler MC = GameObject.FindWithTag("MainSystem").GetComponent<MenusControler>();
         Debug.Log(MC.GameIsPaused);

         if (MC.GameIsPaused) return;*/
        //if(LastAudio == DamageSound && audioSource.isPlaying) return;
        if(LastAudio != sound && (LastAudio != DamageSound || sound == DeathSound)) audioSource.Stop();
        LastAudio = audioSource.isPlaying ? sound : null;

        if (LastAudio == sound) return;
        LastAudio = sound;
        audioSource.PlayOneShot(sound);
    }

    public void ReproduceClick(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
