using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIMenu : MonoBehaviour
{
    [SerializeField] AudioClip ClickSound;
    AudioSettings AS;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btnMainMenu = root.Q<Button>("mainmenu");
        Button btnQuit = root.Q<Button>("quit");

        btnMainMenu.clicked += () => ReturnMenu();
        btnQuit.clicked += () => QuitGame();
    }

    private void Start()
    {
        AS = GameObject.FindWithTag("AudioManager").GetComponent<AudioSettings>();
        AS.WinStartMusic();
        
        int GamePlaying = PlayerPrefs.GetInt("LastGame");
        PlayerPrefs.SetFloat("CPX" + GamePlaying, 0);
        PlayerPrefs.SetFloat("CPY" + GamePlaying, 0);
        PlayerPrefs.SetFloat("SpawnConter" + GamePlaying, 0);
        PlayerPrefs.SetInt("SpawnActive" + GamePlaying, 0);
        PlayerPrefs.SetInt("LastGame" + GamePlaying, 0);
        PlayerPrefs.SetInt("Life" + GamePlaying, 3);
        PlayerPrefs.SetFloat("Heal" + GamePlaying, 100);
        PlayerPrefs.SetFloat("Soul" + GamePlaying, 0);
        PlayerPrefs.SetInt("LastGame", 0);

    } 

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMenu()
    {
        AudioManager.Instance.ReproduceClick(ClickSound);
        SceneManager.LoadScene("UI_MainMenu");
        AS.StartStartMusic();
    }
}
