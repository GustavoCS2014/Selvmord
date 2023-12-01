using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings audioSettings;

    [Header("Information - Read Only from inspector")]
    [SerializeField]
    private float masterVolume;
    [SerializeField]
    private float musicVolume;
    [SerializeField]
    private float sfxVolume;

    private float ValuemusicVolume;
    private float ValuesfxVolume;

    float masterDefaultVolume = 0.5f;
    float musicDefaultVolume = 0.5f;
    float sfxDefaultVolume = 0.5f;

    string masterVolumeDataName = "master-volume";
    string musicVolumeDataName = "music-volume";
    string sfxVolumeDataName = "sfx-volume";

    List<AudioSource> musicAudioSources;
    List<AudioSource> sfxAudioSources;

    [SerializeField]
    private int musicAudioSourcesCount = 0;
    [SerializeField]
    private int sfxAudioSourcesCount = 0;

    [Space(5)]
    [Header("---- Music ----")]

    public AudioClip StartMusic;
    public AudioClip GameMusic;
    public AudioClip BossMusic;
    public AudioClip WinMusic;
    public AudioClip LoseMusic;

    private AudioClip CurrentMusic;

    private GameObject AudioGroup;
    private AudioSource Music;

    private bool isPlaying = false;
    MenusControler MC;
    private void Awake()
    {
        audioSettings = this;
        musicAudioSources = new List<AudioSource>();
        sfxAudioSources = new List<AudioSource>();
        LoadSavedSettings();
        MC = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MenusControler>();
    }

    private void Start()
    {
        Invoke("PlayMusic", 0.5f);
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (!Music.isPlaying)
        {
            Music.PlayOneShot(CurrentMusic);
        }
    }

    void PlayMusic()
    {
        AudioGroup = GameObject.FindWithTag("Music");
        Music = AudioGroup.GetComponent<AudioSource>();
        Music.Play();
        isPlaying= true;
        CurrentMusic = StartMusic;
    }

    private void PlayMusic(AudioClip sound)
    {
        CurrentMusic = Music.isPlaying ? sound : null;

        if (CurrentMusic == sound) return;
        CurrentMusic = sound;
        Music.PlayOneShot(sound);
    }

    void LoadSavedSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(masterVolumeDataName, masterDefaultVolume);
        musicVolume = PlayerPrefs.GetFloat(musicVolumeDataName, musicDefaultVolume);
        sfxVolume = PlayerPrefs.GetFloat(sfxVolumeDataName, sfxDefaultVolume);
    }
    
    public void ChangeMasterVolume(float newVolume)
    {
        masterVolume = newVolume;
        PlayerPrefs.SetFloat(masterVolumeDataName, masterVolume);
        
        SetVolumeToAudioSources(musicAudioSources, musicVolume * masterVolume);
        SetVolumeToAudioSources(sfxAudioSources, sfxVolume * masterVolume);
    }

    public void ChangeMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        PlayerPrefs.SetFloat(musicVolumeDataName, musicVolume);
        SetVolumeToAudioSources(musicAudioSources, masterVolume * musicVolume);
    }


    public void ChangSFXVolume(float newVolume)
    {
        sfxVolume = newVolume;
        PlayerPrefs.SetFloat(sfxVolumeDataName, sfxVolume);
        SetVolumeToAudioSources(sfxAudioSources, masterVolume * sfxVolume);
    }

    void SetVolumeToAudioSources(List<AudioSource> audioSources, float volume)
    {
        foreach (AudioSource a in audioSources)
        {
            a.volume = volume;
        }
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume * masterVolume;
    }

    public float GetMusicVolumeUI()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume * masterVolume;
    }

    public float GetSFXVolumeUI()
    {
        return sfxVolume;
    }

    public void AddMeToMusicAudioSources(AudioSource a)
    {
        musicAudioSources.Add(a);
        musicAudioSourcesCount = musicAudioSources.Count;
    }

    public void RemoveMeFromMusicAudioSources(AudioSource a)
    {
        musicAudioSources.Remove(a);
        musicAudioSourcesCount = musicAudioSources.Count;
    }
    public void AddMeToSFXAudioSources(AudioSource a)
    {
        sfxAudioSources.Add(a);
        sfxAudioSourcesCount = sfxAudioSources.Count;
    }

    public void RemoveMeFromSFXAudioSources(AudioSource a)
    {
        sfxAudioSources.Remove(a);
        sfxAudioSourcesCount = sfxAudioSources.Count;
    }

    public void StopMusic()
    {
        isPlaying = false;
        Music.Pause();

    }

    public void ResumeMusic()
    {
        Music.UnPause();
        isPlaying = true;
    }

    public void MusicStartGame()
    {
        Music.Stop();
        PlayMusic(GameMusic);
        CurrentMusic = GameMusic;
    }

    public void BossStartMusic()
    {
        Music.Stop();
        PlayMusic(BossMusic);
        CurrentMusic = BossMusic;
    }

    public void WinStartMusic()
    {
        Music.Stop();
        PlayMusic(WinMusic);
        CurrentMusic = WinMusic;
    }

    public void lostStartMusic()
    {
        Music.Stop();
        PlayMusic(LoseMusic);
        CurrentMusic = LoseMusic;
    }

    public void StartStartMusic()
    {
        Music.Stop();
        PlayMusic(StartMusic);
        CurrentMusic = StartMusic;
    }
}
