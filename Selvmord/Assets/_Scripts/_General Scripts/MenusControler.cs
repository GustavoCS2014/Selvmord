using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusControler : MonoBehaviour
{
    public bool GameIsPaused = false;
    [SerializeField] GameObject menuPause;

    // Update is called once per frame
    void Update()
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
    }

    public void ResetSpawn()
    {
        
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("CPX", 0);
        PlayerPrefs.SetFloat("CPY", 0);
        PlayerPrefs.SetFloat("SpawnConter", 0);
        PlayerPrefs.SetInt("SpawnActive", 0);
        SceneManager.LoadScene("Game");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("CPX", 0);
        PlayerPrefs.SetFloat("CPY",0);
        PlayerPrefs.SetFloat("SpawnConter", 0);
        PlayerPrefs.SetInt("SpawnActive", 0);
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("CPX",0);
        PlayerPrefs.SetFloat("CPY", 0);
        PlayerPrefs.SetFloat("SpawnConter", 0);
        PlayerPrefs.SetInt("SpawnActive", 0);
        SceneManager.LoadScene("GameOver");
    }
    public void EndGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("CPX", 0);
        PlayerPrefs.SetFloat("CPY", 0);
        PlayerPrefs.SetFloat("SpawnConter", 0);
        PlayerPrefs.SetInt("SpawnActive", 0);
        SceneManager.LoadScene("EndGame");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("CPX", 0);
        PlayerPrefs.SetFloat("CPY", 0);
        PlayerPrefs.SetFloat("SpawnConter", 0);
        PlayerPrefs.SetInt("SpawnActive", 0);
        Application.Quit();
    }

    public void Test()
    {
        Debug.Log("Hola_");
    }
}
