using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    private MainSystem MS;

    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("mainSystem").GetComponent<MainSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MS.AddSoul(1);
            gameObject.SetActive(false);
        }
    }
}
