using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeReturn : MonoBehaviour
{
    [SerializeField] Transform CheckPoint;
    [SerializeField] Transform Player;

    MainSystem MS;
    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MS.DamagePlayer(30, collision.GetContact(0).normal);
            Player.position = CheckPoint.position;
        }
    }
}
