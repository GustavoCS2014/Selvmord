using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : MonoBehaviour
{
    MainSystem MS;
    // Start is called before the first frame update
    void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        Invoke("TimeLife",3f);
    }
    void TimeLife()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MS.DamagePlayer(30, collision.GetContact(0).normal);
        }
    }
}
