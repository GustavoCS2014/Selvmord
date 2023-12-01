using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UILose : MonoBehaviour
{
    [SerializeField] AudioClip ClickSound;
    AudioSettings AS;


    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button BtnRetry = root.Q<Button>("retry");
        Button BtnQuit = root.Q<Button>("quit");
        Button BtnMenu = root.Q<Button>("Menu");

        BtnRetry.clicked += () => TryAgain();
        BtnQuit.clicked += () => ReturnMenu();
        BtnMenu.clicked += () => QuitGame();
    }

    private void Start()
    {
        AS = GameObject.FindWithTag("AudioManager").GetComponent<AudioSettings>();
        AS.lostStartMusic();

        int GamePlaying = PlayerPrefs.GetInt("LastGame");
        PlayerPrefs.SetFloat("CPX" + GamePlaying, 0);
        PlayerPrefs.SetFloat("CPY" + GamePlaying, 0);
        PlayerPrefs.SetFloat("SpawnConter" + GamePlaying, 0);
        PlayerPrefs.SetInt("SpawnActive" + GamePlaying, 0);
        PlayerPrefs.SetInt("LastGame" + GamePlaying, 0);
        PlayerPrefs.SetInt("Life" + GamePlaying, 3);
        PlayerPrefs.SetFloat("Heal" + GamePlaying, 100);
        PlayerPrefs.SetFloat("Soul" + GamePlaying, 0);

    }

    public void QuitGame()
    {

        PlayerPrefs.SetInt("LastGame", 0);
        Application.Quit();
    }

    public void ReturnMenu()
    {
        PlayerPrefs.SetInt("LastGame", 0);
        AudioManager.Instance.ReproduceClick(ClickSound);
        SceneManager.LoadScene("UI_MainMenu");
        AS.StartStartMusic();
    }

    public void TryAgain()
    {
        AudioManager.Instance.ReproduceClick(ClickSound);
        SceneManager.LoadScene("level1-1");
        AS.MusicStartGame();
        
    }
}
