using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesMenu : MonoBehaviour
{
    public TextMeshProUGUI [] Game1;
    public TextMeshProUGUI [] Game2;
    public TextMeshProUGUI [] Game3;

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
    }

    public void Gameplay1()
    {
        if (PlayerPrefs.GetInt("GameUI") == 1)
        {
            PlayerPrefs.SetInt("LastGame", 1);
            SceneManager.LoadScene("level1-1");
        }
        else if (PlayerPrefs.GetInt("GameUI") == 2)
        {
            PlayerPrefs.SetInt("LastGame", 1);
            SceneManager.LoadScene("level1-1");
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
            PlayerPrefs.SetInt("LastGame", 2);
            SceneManager.LoadScene("level1-1");
        }
        else if (PlayerPrefs.GetInt("GameUI") == 2)
        {
            PlayerPrefs.SetInt("LastGame", 2);
            SceneManager.LoadScene("level1-1");
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
            PlayerPrefs.SetInt("LastGame", 3);
            SceneManager.LoadScene("level1-1");
        }
        else if (PlayerPrefs.GetInt("GameUI") == 2)
        {
            PlayerPrefs.SetInt("LastGame", 3);
            SceneManager.LoadScene("level1-1");
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
        SceneManager.LoadScene("UI_MainMenu");
    }
}
