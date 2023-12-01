using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlyEnemy : MonoBehaviour
{
    [SerializeField] float speedBullet;
    [SerializeField] ParticleSystem ParticleEmitter;
    private bool crashed;
    private Rigidbody2D rb2d;
    MainSystem MS;
    

    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(crashed && ParticleEmitter.isStopped) {

            GetComponent<Collider2D>().enabled = true;
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speedBullet * Time.deltaTime);
        Invoke("Destroy", 3f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            ParticleEmitter.Play();
            GetComponent<Collider2D>().enabled = false;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            crashed = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ParticleEmitter.Play();
            GetComponent<Collider2D>().enabled = false;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            crashed = true;
            MS.DamagePlayer(20, new Vector2(0,0));

        }
    }

    
}
