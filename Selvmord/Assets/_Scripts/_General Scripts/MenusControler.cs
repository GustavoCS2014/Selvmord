using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusControler : MonoBehaviour
{
    [SerializeField] bool menusEnabled = false;

    public bool GameIsPaused;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject SaveGame;
    float con;

    [Space  (5)]
    [Header("---- Sound Efects ----")]
    [SerializeField] AudioClip ClickSound;

    GameObject AsGO;
    AudioSettings AS;

    AudioSource AuSourse;

    private void Awake()
    {
        AsGO = GameObject.FindWithTag("AudioManager");
        AS = AsGO.GetComponent<AudioSettings>();
        AuSourse = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (AuSourse == null) return;
        else AuSourse.Pause();
    }

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
        AS.ResumeMusic();
        AuSourse.Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        menuPause.SetActive(true);
        GameIsPaused = true;
        menusEnabled = true;
        AS.StopMusic();
        AuSourse.Play();
    }

    public void PauseUI()
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
        audioManager.Instance.ReproduceClick(ClickSound);
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void Continue()
    {
        audioManager.Instance.ReproduceClick(ClickSound);

        if (PlayerPrefs.GetInt("LastGame") == 0)
        {
            menuPause.SetActive(true);
            GameIsPaused = true;
        }
        else
        {
            AS.MusicStartGame();
            SceneManager.LoadScene("level1-1");
        }
       
    }

    public void NewGame()
    {
        ClickSoundBottom();
        PlayerPrefs.SetInt ("GameUI", 1);
        SceneManager.LoadScene("UI_Games");
    }

    public void LoadGame()
    {
        audioManager.Instance.ReproduceClick(ClickSound);
        PlayerPrefs.SetInt("GameUI", 2);
        SceneManager.LoadScene("UI_Games");
    }

    public void Settings()
    {
        audioManager.Instance.ReproduceClick(ClickSound);
        SceneManager.LoadScene("AudioSystem");
    }

    public void Return()
    {
        audioManager.Instance.ReproduceClick(ClickSound);
        SceneManager.LoadScene("UI_MainMenu");
    }

    public void OpenSaveMenu()
    {
        
        menuPause.SetActive(false);
        SaveGame.SetActive(true);
    }

    private void ClickSoundBottom()
    {
        audioManager.Instance.ReproduceClick(ClickSound);
    }
}
