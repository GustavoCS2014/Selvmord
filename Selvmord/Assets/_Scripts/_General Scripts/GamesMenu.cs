using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesMenu : MonoBehaviour
{
    public TextMeshProUGUI[] Game1;
    public TextMeshProUGUI[] Game2;
    public TextMeshProUGUI[] Game3;

    [SerializeField] private GameObject YesNoUI;
    private int game;

    [Space(5)]
    [Header("---- Sound Efects ----")]
    [SerializeField] AudioClip ClickSound;
    [SerializeField] AudioClip CloseSound;

    AudioSettings AS;
    void Start()
    {
        Game1[0].text = "Heal: " + PlayerPrefs.GetFloat("Heal" + 1);
        Game1[1].text = "Souls: " + PlayerPrefs.GetFloat("Soul" + 1);
        Game1[2].text = "Lifes: " + PlayerPrefs.GetInt("Life" + 1);

        Game2[0].text = "Heal: " + PlayerPrefs.GetFloat("Heal" + 2);
        Game2[1].text = "Souls: " + PlayerPrefs.GetFloat("Soul" + 2);
        Game2[2].text = "Lifes: " + PlayerPrefs.GetInt("Life" + 2);

        Game3[0].text = "Heal: " + PlayerPrefs.GetFloat("Heal" + 3);
        Game3[1].text = "Souls: " + PlayerPrefs.GetFloat("Soul" + 3);
        Game3[2].text = "Lifes: " + PlayerPrefs.GetInt("Life" + 3);

        AS = GameObject.FindWithTag("AudioManager").GetComponent<AudioSettings>();
    }

    public void Gameplay1()
    {
        if (PlayerPrefs.GetInt("GameUI") == 1)
        {
            AudioManager.Instance.ReproduceClick(ClickSound);
            YesNoUI.SetActive(true);
            game = 1;
        }
        else if (PlayerPrefs.GetInt("GameUI") == 2)
        {
            PlayerPrefs.SetInt("LastGame", 1);
            LoadLevel();
            AudioManager.Instance.ReproduceClick(ClickSound);
        }
        else
        {
            Debug.Log("Error");
            Debug.Log(PlayerPrefs.GetInt("GameUI"));
            Debug.Log(PlayerPrefs.GetInt("LastGame"));
        }
    }

    public void Gameplay2()
    {
        if (PlayerPrefs.GetInt("GameUI") == 1)
        {
            AudioManager.Instance.ReproduceClick(ClickSound);
            YesNoUI.SetActive(true);
            game = 2;
        }
        else if (PlayerPrefs.GetInt("GameUI") == 2)
        {
            PlayerPrefs.SetInt("LastGame", 2);
            LoadLevel();
            AudioManager.Instance.ReproduceClick(ClickSound);
        }
        else
        {
            Debug.Log("Error");
            Debug.Log(PlayerPrefs.GetInt("GameUI"));
            Debug.Log(PlayerPrefs.GetInt("LastGame"));
        }
    }

    public void Gameplay3()
    {
        if (PlayerPrefs.GetInt("GameUI") == 1)
        {
            AudioManager.Instance.ReproduceClick(ClickSound);
            YesNoUI.SetActive(true);
            game = 3;
        }
        else if (PlayerPrefs.GetInt("GameUI") == 2)
        {
            PlayerPrefs.SetInt("LastGame", 3);
            LoadLevel();
            AudioManager.Instance.ReproduceClick(ClickSound);
        }
        else
        {
            Debug.Log("Error");
            Debug.Log(PlayerPrefs.GetInt("GameUI"));
            Debug.Log(PlayerPrefs.GetInt("LastGame"));
        }
    }

    public void Return()
    {
        AudioManager.Instance.ReproduceClick(CloseSound);
        SceneManager.LoadScene("UI_MainMenu");
    }

    public void CloseUI()
    {
        AudioManager.Instance.ReproduceClick(CloseSound);
        YesNoUI.SetActive(false);
    }

    public void GamePlay()
    {
        PlayerPrefs.SetInt("FistStart", 0);
        AudioManager.Instance.ReproduceClick(ClickSound);
        ResetGame(game);
        LoadLevel();
    }

    void ResetGame(int game)
    {
        PlayerPrefs.SetInt("LastGame", game);
        PlayerPrefs.SetFloat("CPX" + game, 0);
        PlayerPrefs.SetFloat("CPY" + game, 0);
        PlayerPrefs.SetInt("SpawnConter" + game, 0);
        PlayerPrefs.SetInt("SpawnActive" + game, 0);
        PlayerPrefs.SetInt("Life" + game, 3);
        PlayerPrefs.SetFloat("Heal" + game, 100);
        PlayerPrefs.SetFloat("Soul" + game, 0);
    }

    void LoadLevel()
    {
        AS.MusicStartGame();
        SceneManager.LoadScene("level1-1");
    }
}