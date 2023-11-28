using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    [SerializeField] GameObject[] Enemys;
    [SerializeField] GameObject[] Spawns;
    [SerializeField] GameObject[] Items;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] int distanceActivation;
    [SerializeField] int spawnConter;
    private bool active = true;

    MainSystem MS;
    Transform Player;

    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        ResertSpawns();
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (MS.Health < 0)
        {
            active = true;

            ResertSpawns();
        }

        if (active)
        {
            if (Mathf.Abs(Player.transform.position.x - transform.position.x) < distanceActivation)
            {
                RespawnEnemy();
                active = false;
            }
        }

        if (PlayerPrefs.GetInt("SpawnConter") > spawnConter)
        {
            SpawnPoint.SetActive(false);
        }
    }

    private void RespawnEnemy()
    {

        SpawnPoint.SetActive(true);

        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].SetActive(true);
            Enemys[i].transform.position = Spawns[i].transform.position;
        }
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].SetActive(true);
        }
    }

    private void ResertSpawns() {

        if (PlayerPrefs.GetInt("SpawnConter") != spawnConter)
        {
            SpawnPoint.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanceActivation);
    }
}
