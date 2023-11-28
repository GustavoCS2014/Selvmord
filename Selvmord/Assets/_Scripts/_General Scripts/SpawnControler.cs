using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnControler : MonoBehaviour
{
    #region Variable
    //----------- Help variable ------------
    int GamePlaying;
    MainSystem MS;
    float health;

    //------------ HUD action --------------
    [SerializeField] GameObject FBotom;
    [SerializeField] Image FBar;

    //--------- Variable action ------------
    float conSec = 0;
    bool In = false;

    #endregion

    #region Start Method
    void Start()
    {
        GamePlaying = PlayerPrefs.GetInt("LastGame");
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        RespawnPlayer();

    }
    #endregion

    #region Update Method
    // Update is called once per frame
    void Update()
    {

        //------- HUD interactive ---------
        if (In)
        {
            if (conSec > 0)
            {
                conSec -= Time.deltaTime;
            }
        }

        //--------- HUD Actives ----------
        FBar.fillAmount = conSec / 1;

        health = MS.Health;

        if (health < 0)
        {
            RespawnPlayer();
        }

    }
    #endregion

    #region Spawn Method
    private void RespawnPlayer()
    {
        transform.position = new Vector2(PlayerPrefs.GetFloat("CPX"+GamePlaying), PlayerPrefs.GetFloat("CPY" + GamePlaying));
    }

    public void ReachedCheckPoint(float x, float y)
    {
        PlayerPrefs.SetFloat("CPX" + GamePlaying, x);
        PlayerPrefs.SetFloat("CPY" + GamePlaying, y);
    }
    #endregion

    #region Collision Method
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "FallDetector")
        {
            MS.DamagePlayer(101, new Vector2(0, 0));
            RespawnPlayer();
        }
    }
    #endregion

    #region Trigger Method
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
        {
            FBotom.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
        {
            FBotom.SetActive(false);
            In = false;
            conSec = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
        {
            In = true;

            if (Input.GetKey(KeyCode.F))
            {
                conSec += 2 * (Time.deltaTime);
            }
        }
    }
    #endregion
}
