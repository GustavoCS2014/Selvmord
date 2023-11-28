using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusControler : MonoBehaviour
{
    [SerializeField] bool menusEnabled = false;

    public bool GameIsPaused = false;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject SaveGame;
    float con;

    // Update is called once per frame
    void Update()
    {
        if (menusEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        if (con > 3)
        {
            menuPause.SetActive(false);
            GameIsPaused = false;

        }
        else if (GameIsPaused)
        {
            con += Time.deltaTime;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        menuPause.SetActive(false);
        GameIsPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        menuPause.SetActive(true);
        GameIsPaused = true;
        menusEnabled = true;
    }

    public void menuPauseClose()
    {
        menusEnabled = false;
        menuPause.SetActive(false);
    }
        
    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void Test()
    {
        Debug.Log("Hola_");
    }

    public void Continue()
    {
        if (PlayerPrefs.GetInt("LastGame") == 0)
        {
            menuPause.SetActive(true);
            GameIsPaused = true;
        }
        else
        {
            SceneManager.LoadScene("level1-1");
        }
       
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("GameUI", 1);
        SceneManager.LoadScene("UI_Games");
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("GameUI", 2);
        SceneManager.LoadScene("UI_Games");
    }

    public void Settings()
    {
        SceneManager.LoadScene("AudioSystem");
    }

    public void Return()
    {
        SceneManager.LoadScene("UI_MainMenu");
    }

    public void OpenSaveMenu()
    {
        menuPause.SetActive(false);
        SaveGame.SetActive(true);
    }
}
