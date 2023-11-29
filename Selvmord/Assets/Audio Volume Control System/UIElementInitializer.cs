using UnityEngine;
using UnityEngine.UI;

public class UIElementInitializer : MonoBehaviour
{
    GameObject AsGO;
    AudioSettings AS;

    public enum UIElementType { 
        
        MASTER_Slider,
        SFX_Slider,
        MUSIC_Slider

    }

    public UIElementType type;

    Slider slider;

    private void Start()
    {
        AsGO = GameObject.FindWithTag("AudioManager");
        AS = AsGO.GetComponent<AudioSettings>();
        
        float Master = AS.GetMasterVolume();
        float SFX = AS.GetSFXVolumeUI();
        float Music = AS.GetMusicVolumeUI();

        switch (type)
        {
            case UIElementType.SFX_Slider:
                slider = GetComponent<Slider>();
                slider.value = SFX;
                break;
            case UIElementType.MUSIC_Slider:
                slider = GetComponent<Slider>();
                slider.value = Music;
                break;
           case UIElementType.MASTER_Slider:
                slider = GetComponent<Slider>();
                slider.value = Master;
                break;
        }

    }

}
