using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelpSettings : MonoBehaviour
{
    [Header("Information - Read Only from inspector")]
    [SerializeField]
    private float masterVolume;
    [SerializeField]
    private float musicVolume;
    [SerializeField]
    private float sfxVolume;

    private float ValuemusicVolume;
    private float ValuesfxVolume;

    float masterDefaultVolume = 1f;
    float musicDefaultVolume = 1f;
    float sfxDefaultVolume = 1f;

    string masterVolumeDataName = "master-volume";
    string musicVolumeDataName = "music-volume";
    string sfxVolumeDataName = "sfx-volume";

    GameObject AsGO;
    AudioSettings AS;

    // Start is called before the first frame update
    void Awake()
    {
        masterVolume = PlayerPrefs.GetFloat(masterVolumeDataName, masterDefaultVolume);
        musicVolume = PlayerPrefs.GetFloat(musicVolumeDataName, musicDefaultVolume);
        sfxVolume = PlayerPrefs.GetFloat(sfxVolumeDataName, sfxDefaultVolume);

        AsGO = GameObject.FindWithTag("AudioManager");
        AS = AsGO.GetComponent<AudioSettings>();
    }
    public void ChangeMasterVolume(float newVolume)
    {
        AS.ChangeMasterVolume(newVolume);
    }

    public void ChangeMusicVolume(float newVolume)
    {
        AS.ChangeMusicVolume(newVolume);
    }


    public void ChangSFXVolume(float newVolume)
    {
       AS.ChangSFXVolume(newVolume);
    }
}
