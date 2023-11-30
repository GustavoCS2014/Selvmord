using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject FBotom;
    float conSec = 0;
    bool Enter = true;

    int GamePlaying;
    MainSystem MS;

    private void Start()
    {
        GamePlaying = PlayerPrefs.GetInt("LastGame");
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        PlayerPrefs.SetInt("SpawnConter", PlayerPrefs.GetInt("SpawnActive" + GamePlaying));

    }
    void Update()
    {
        if (conSec > 0)
        {
            conSec -= Time.deltaTime;
        }

        if (MS.Health < 0)
        {
            Enter = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            FBotom.SetActive(true);

            if (Enter)
            {
                int con = 1 + PlayerPrefs.GetInt("SpawnConter");
                PlayerPrefs.SetInt("SpawnConter", con);
                Enter = false;
            }

            MS.WallActive(PlayerPrefs.GetInt("SpawnConter"));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FBotom.SetActive(false);
            conSec = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            if (Input.GetKey(KeyCode.F))
            {
                conSec += 2 * (Time.deltaTime);

                if (conSec / 1 >= 1)
                {
                    collision.GetComponent<SpawnControler>().ReachedCheckPoint(transform.position.x, transform.position.y+2);
                    MS.AddCheckPoint(PlayerPrefs.GetInt("SpawnConter"));
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
