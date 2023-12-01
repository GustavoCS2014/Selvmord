using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UlMenuPause : MonoBehaviour
{

    [SerializeField] MenusControler MC;
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject RetrunToMenu;

    [Space(5)]
    [Header("---- Sound Efects ----")]
    [SerializeField] AudioClip ClickSound;
    [SerializeField] AudioClip ReturnSound;

    AudioSettings AS;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btnContinue = root.Q<Button>("resume");
        Button btnNewGame = root.Q<Button>("settings");
        Button btnLoad = root.Q<Button>("mainmenu");

        btnContinue.clicked += () => MC.Resume();
        btnNewGame.clicked += () => settings();
        btnLoad.clicked += () => ReturnToMenu();

        
    }

    private void Start()
    {
        AS = GameObject.FindWithTag("AudioManager").GetComponent<AudioSettings>();
    }

    void settings()
    {
        Settings.SetActive(true);
        MC.menuPauseClose();
        AudioManager.Instance.ReproduceClick(ClickSound);
    }

    public void CloseSettings()
    {
        Settings.SetActive(false);
        AudioManager.Instance.ReproduceClick(ReturnSound);
    }

    void ReturnToMenu()
    {
        MC.menuPauseClose();
        RetrunToMenu.SetActive(true);
        AudioManager.Instance.ReproduceClick(ReturnSound);
    }
    public void pause()
    {
        Settings.SetActive(false);
        MC.PauseUI();
        AudioManager.Instance.ReproduceClick(ReturnSound);
    }

    public void YesOption() 
    {
        SceneManager.LoadScene("UI_MainMenu");
        AudioManager.Instance.ReproduceClick(ClickSound);
        Time.timeScale = 1f;
        AS.StartStartMusic();
    }

    public void NoOption()
    {
        int game = PlayerPrefs.GetInt("LastGame");
        PlayerPrefs.SetInt("LastGame", 0);
        PlayerPrefs.SetFloat("CPX" + game, 0);
        PlayerPrefs.SetFloat("CPY" + game, 0);
        PlayerPrefs.SetInt("SpawnConter" + game, 0);
        PlayerPrefs.SetInt("SpawnActive" + game, 0);
        PlayerPrefs.SetInt("Life" + game, 3);
        PlayerPrefs.SetFloat("Heal" + game, 100);
        PlayerPrefs.SetFloat("Soul" + game, 0);
        SceneManager.LoadScene("UI_MainMenu");
        AudioManager.Instance.ReproduceClick(ClickSound);
        Time.timeScale = 1f;
        AS.StartStartMusic();
    }

    public void CloseOptions()
    {
        RetrunToMenu.SetActive(false);
        MC.Pause();
        AudioManager.Instance.ReproduceClick(ReturnSound);
    }
}
