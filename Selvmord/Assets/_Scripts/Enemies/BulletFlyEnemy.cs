using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlyEnemy : MonoBehaviour
{
    [SerializeField] float speedBullet;
    MainSystem MS;

    private void Start()
    {
        MS = GameObject.FindGameObjectWithTag("mainSystem").GetComponent<MainSystem>();
    }
    void Update()
    {
        transform.Translate(Vector2.right * speedBullet * Time.deltaTime);
        Invoke("TimeLife", 3f);
    }

    void TimeLife()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MS.DamagePlayer(30, new Vector2(0,-35));
            gameObject.SetActive(false);
        }
    }
}
