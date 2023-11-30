using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeReturn : MonoBehaviour
{

    MainSystem MS;
    SpawnControler SC;
    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        SC = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnControler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MS.DamageSpikesReturn();
            SC.ReturnPlayerSpawn();
        }
    }
}
