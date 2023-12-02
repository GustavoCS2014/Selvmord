using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    MainSystem MS;
    SpawnControler SC;

    private float stayTime = 1f;
    private float counter = 0;
    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        SC = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnControler>();
        counter = stayTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           MS.DamagePlayer(30, collision.GetContact(0).normal);
        }
    }
    private void OnCollisionStay2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            counter -= Time.deltaTime;
            if(counter < 0)
                SC.ReturnPlayerSpawn();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            counter = stayTime;
        }
    }
}
