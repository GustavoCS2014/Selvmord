using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal : MonoBehaviour
{
    private MainSystem MS;

    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MS.GetHealing();
            gameObject.SetActive(false);
        }
    }
}
