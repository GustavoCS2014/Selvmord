using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class MainSystem : MonoBehaviour
{
    #region Variables
    //----------------  HUD ---------------------
    [SerializeField] Image[] HealthBar = new Image[5];
    [SerializeField] Image SoulBar;

    [SerializeField] GameObject E;

    //-------- Principal Variables MS -----------
    int GamePlaying;

    public float Health;
    int soul;
    int life;

    // ---------- Extra Variables MS -----------
    public float MaxHealth;
    public float MaxSoul;
    private float CurrentHealth;
    private float CurrentSoul;
    bool healing = false;
    bool GetingDamage = false;
    bool GetSoul = false;
    bool LoseSoul = false;

    // ----------- Activate Spawn --------------
    [SerializeField] public GameObject[] CheckPointsGO;
    [SerializeField] GameObject[] Walls;
    int conNum = 0;

    // ----------- Loset Control ---------------
    PlayerMovement PM;
    [SerializeField] float TimeLostControl;
    [SerializeField] float TimeImmortal;
    bool inmortal = false;
    #endregion

    #region Start Method
    private void Start()
    {
        ReloundPlayerPrefabs();
        InputManager.inputsActive = true;
        // -------------- HUD -----------------
        CurrentHealth = Health;

        // -------- Activate spawn ------------
        SpawnActive();
        WallDesactive();
        WallActive(PlayerPrefs.GetInt("SpawnActive"+GamePlaying));

        // ------ Get Player Movement --------
        PM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        Physics2D.IgnoreLayerCollision(7, 8, false);

        if(Health < 0)
        {
            E.SetActive(true);
            InputManager.Dead = true;
        }
    }
    #endregion

    #region Update Method
    void Update()
    {

        //------------ Testing area -------------
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    AddSoul(5);
        //}

        //---------------- HUD  ----------------

        SoulBar.fillAmount = CurrentSoul / MaxSoul;

        HealthBar[life - 1].fillAmount = CurrentHealth / MaxHealth;
        for (int i = 0; i < HealthBar.Length; i++)
        {
            if (i > life-1)
            {
                HealthBar[i].fillAmount = 0;
            }else if(i < life - 1) {
                HealthBar[i].fillAmount = 100;
            }
        }

        if(Health < 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetHealing();
            }
        }

        HealingDamageEfect();
        SoulsEfect();
    }
    #endregion

    #region Player Effects

    void ReloundPlayerPrefabs()
    {
        GamePlaying = PlayerPrefs.GetInt("LastGame");
        life = PlayerPrefs.GetInt("Life" + GamePlaying);
        Health = PlayerPrefs.GetFloat("Heal" + GamePlaying);
        soul = PlayerPrefs.GetInt("Soul" + GamePlaying);
    }

    void HealingDamageEfect()
    {
        //---------- Healing Effect -----------
        if (healing)
        {
            CurrentHealth += Time.deltaTime * 100;

            if (CurrentHealth >= Health)
            {
                healing = false;
            }
        }

        //---------- Damage Effect ------------
        if (GetingDamage)
        {
            CurrentHealth -= Time.deltaTime * 100;

            if (CurrentHealth <= Health)
            {
                CurrentHealth = Health;
                GetingDamage = false;
            }
        }
    }

    void SoulsEfect()
    {

        //---------- GetSouls Effect -----------
        if (GetSoul)
        {
            CurrentSoul += Time.deltaTime * 10f;

            if (CurrentSoul >= soul)
            {
                CurrentSoul = soul;
                GetSoul = false;
            }
        }

        //---------- LoseSouls10 Effect ------------
        if (LoseSoul)
        {
            CurrentSoul -= Time.deltaTime * 15f;

            if (CurrentSoul <= soul)
            {
                CurrentSoul = soul;
                GetSoul = false;
            }
        }
    }

    public void DamagePlayer(int damage, Vector2 position)
    {
        if (inmortal)
        {
            return;
        }

        Health -= damage;

        EventManager.Instance.DamageTaken();

        if (Health < 0 && life == 1)
        {
            GameOver();
            return;
        }

        if (Health < 0)
        {

            Health = -1;
            InputManager.Dead = true;
            E.SetActive(true);
            WallDesactive();
            PlayerPrefs.SetInt("SpawnConter", PlayerPrefs.GetInt("SpawnActive" + GamePlaying));
        }

        StartCoroutine(LostControl());
        StartCoroutine(DesactiveColision());

        PM.rebound(position);

        PlayerPrefs.SetFloat("Heal" + GamePlaying, Health);

        GetingDamage = true;
        healing = false;
    }

    public void DamageSpikesReturn()
    {
        if (inmortal)
        {
            return;
        }

        Health -= 10;
        EventManager.Instance.DamageTaken();

        if (Health < 0 && life == 1)
        {
            GameOver();
            return;
        }

        if (Health < 0)
        {
            Health = -1;
            InputManager.Dead = true;
            E.SetActive(true);
            WallDesactive();
            PlayerPrefs.SetInt("SpawnConter", PlayerPrefs.GetInt("SpawnActive" + GamePlaying));
        }

        PlayerPrefs.SetFloat("Heal" + GamePlaying, Health);
        GetingDamage = true;
        healing = false;
    }

    public void GetHealing()
    {
        //Provicional

        if (Health < 0)

        {
            InputManager.Dead = false;
            RemoveLife(1);
            RemoveSoul(5);
            E.SetActive(false);
            Health = 100;
            CurrentHealth= Health;

            PlayerPrefs.SetInt("Life" + GamePlaying, life);
            PlayerPrefs.SetFloat("Heal" + GamePlaying, Health);
            PlayerPrefs.SetFloat("Soul" + GamePlaying, soul);

            return;
        }

        //Provicional

        if (Health < 100)
        {
            healing = true;
            GetingDamage = false;
        }
        else
        {
            AddSoul(5);
        }

        Health = 100;
    }

    public void AddLife(int lifes)
    {
        if (life > 4)
        {
            return;
        }

        CurrentHealth = 0;
        healing = true;

        life += lifes;
    }

    public void RemoveLife(int lifes)
    {
        life -= lifes;
    }

    public void AddSoul(int souls)
    {
        soul += souls;
        

        if (soul >= 15)
        {
            soul = soul - 15;
            AddLife(1);
            GetSoul = false;
            LoseSoul = true;
        }
        else
        {
            LoseSoul= false;
            GetSoul = true;
        }
    }

    public void RemoveSoul(int souls)
    {
        soul -= souls;
        LoseSoul = true;

        if (soul < 0)
        {
           soul = 0;
        }
    }

    #endregion

    #region Walls and CheckPoint 
    public void AddCheckPoint(int num)
    {
        PlayerPrefs.SetInt("SpawnActive" + GamePlaying, num);
        SpawnActive();
    }

    private void SpawnActive()
    {
        conNum = PlayerPrefs.GetInt("SpawnActive" + GamePlaying);

        for (int i = conNum; i > 0; i--)
        {
            CheckPointsGO[i - 1].SetActive(false);
        }

    }

    public void WallActive(int checkpoint)
    {
        checkpoint--;

        if (checkpoint >= 0)
        {
            Walls[checkpoint].SetActive(true);
        }

    }

    public void WallDesactive()
    {
        int Point = PlayerPrefs.GetInt("SpawnActive" + GamePlaying);

        for (int i = Point; i < Walls.Length; i++)
        {
            Walls[i].SetActive(false);
        }

    }
    #endregion

    #region Player Lost Control

    private IEnumerator DesactiveColision()
    {
        inmortal= true;
        yield return new WaitForSeconds(TimeImmortal);
        inmortal= false;
    }

    private IEnumerator LostControl()
    {
        InputManager.StopAllInputs(true);
        yield return new WaitForSeconds(TimeLostControl);
        InputManager.StopAllInputs(false);
    }

    private void GameOver()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("CPX" + GamePlaying, 0);
        PlayerPrefs.SetFloat("CPY" + GamePlaying, 0);
        PlayerPrefs.SetFloat("SpawnConter" + GamePlaying, 0);
        PlayerPrefs.SetInt("SpawnActive" + GamePlaying, 0);
        PlayerPrefs.SetInt("LastGame" + GamePlaying, 0);
        PlayerPrefs.SetInt("Life" + GamePlaying, 3);
        PlayerPrefs.SetFloat("Heal" + GamePlaying, 100);
        PlayerPrefs.SetFloat("Soul" + GamePlaying, 0);
        SceneManager.LoadScene("Lose");
    }
    #endregion
}
