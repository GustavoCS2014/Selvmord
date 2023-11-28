using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlyEnemy : MonoBehaviour
{
    [SerializeField] float speedBullet;
    MainSystem MS;
    

    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
       
    }
    void Update()
    {
        transform.Translate(Vector2.right * speedBullet * Time.deltaTime);
        Invoke("Destroy", 4f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            MS.DamagePlayer(30, new Vector2(0,0));
            Destroy(gameObject);

        }
    }

    
}
