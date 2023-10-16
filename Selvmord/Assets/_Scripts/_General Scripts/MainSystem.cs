using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSystem : MonoBehaviour
{
    #region Variables
    //----------------  HUD ---------------------
    [SerializeField] TextMeshProUGUI textSoul;
    [SerializeField] TextMeshProUGUI textLife;
    [SerializeField] Image HealthBar;

    [SerializeField] GameObject E;

    //-------- Principal Variables MS -----------
    public float Health;
    int soul;
    int life = 3;
    // ---------- Extra Variables MS -----------
    public float MaxHealth;
    private float CurrentHealth;
    bool healing = false;
    bool GetingDamage = false;

    // ----------- Activate Spawn --------------
    [SerializeField] GameObject[] CheckPointsGO;
    [SerializeField] GameObject[] Walls;
    int conNum = 0;

    // ----------- Loset Control ---------------
    PlayerMovement PM;
    [SerializeField] float TimeLostControl;
    [SerializeField] float TimeImmortal;
    #endregion

    #region Start Method
    private void Start()
    {
        // -------------- HUD -----------------
        textLife.text = "X " + life;
        textSoul.text = "X " + soul;
        CurrentHealth = Health;

        // -------- Activate spawn ------------
        SpawnActive();
        WallActive(PlayerPrefs.GetInt("SpawnActive"));

        // ------ Get Player Movement --------
        PM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
    #endregion

    #region Update Method
    void Update()
    {

        //------------ Testing area -------------
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetDamage();

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetHealing();
        }

        //---------------- HUD  ----------------
        HealthBar.fillAmount = CurrentHealth / MaxHealth;
        textLife.text = "X " + life;
        textSoul.text = "X " + soul;

        //---------- Healing Effect -----------
        if (healing)
        {
            CurrentHealth += Time.deltaTime * 150;

            if (CurrentHealth >= Health)
            {
                healing = false;
            }
        }

        //---------- Damage Effect ------------
        if (GetingDamage)
        {
            CurrentHealth -= Time.deltaTime * 150;

            if (CurrentHealth <= Health)
            {
                CurrentHealth = Health;
                GetingDamage = false;
            }
        }
    }

    #endregion

    #region Player Effects
    public void DamagePlayer(int damage, Vector2 position)
    {
        Health -= damage;

        if (Health < 0 && life == 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (Health < 0)
        {
            Health = -1;
            E.SetActive(true);
            WallDesactive();
            PlayerPrefs.SetInt("SpawnConter", PlayerPrefs.GetInt("SpawnActive"));
        }

        StartCoroutine(LostControl());
        StartCoroutine(DesactiveColision());

        PM.rebound(position);

        GetingDamage = true;
        healing = false;
    }

    public void GetDamage()
    {
        Health -= 30;

        if (Health < 0 && life == 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (Health < 0)
        {
            Health = -1;
            E.SetActive(true);
            WallDesactive();
            PlayerPrefs.SetInt("SpawnConter", PlayerPrefs.GetInt("SpawnActive"));
        }

        StartCoroutine(LostControl());
        StartCoroutine(DesactiveColision());

        GetingDamage = true;
        healing = false;
    }

    public void GetHealing()
    {
        //Provicional

        if (Health < 0)

        {
            RemoveLife(1);
            RemoveSoul(5);
            E.SetActive(false);
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
        }
    }

    public void RemoveSoul(int souls)
    {
        soul -= souls;
        if (soul < 0)
        {
            soul = 0;
        }
    }

    #endregion

    #region Walls and CheckPoint 
    public void AddCheckPoint(int num)
    {
        PlayerPrefs.SetInt("SpawnActive", num);
        SpawnActive();
    }

    private void SpawnActive()
    {
        conNum = PlayerPrefs.GetInt("SpawnActive");

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
        int Point = PlayerPrefs.GetInt("SpawnActive");

        for (int i = Point; i < Walls.Length; i++)
        {
            Walls[i].SetActive(false);
        }

    }
    #endregion

    #region Player Lost Control

    private IEnumerator DesactiveColision()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        yield return new WaitForSeconds(TimeImmortal);
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    private IEnumerator LostControl()
    {
        InputManager.StopAllInputs(true);
        yield return new WaitForSeconds(TimeLostControl);
        InputManager.StopAllInputs(false);
    }
    #endregion
}
