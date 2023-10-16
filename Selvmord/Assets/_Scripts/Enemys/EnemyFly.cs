using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [SerializeField] Transform[] pointsMovements;
    [SerializeField] Transform shotPoint;
    Transform Player;
    [SerializeField] GameObject bullet;

    [SerializeField] float speed;
    [SerializeField] float playerDetection;
    [SerializeField] float distanceMov;

    [SerializeField] float DistancePointX;
    [SerializeField] float DistancePointY;

    bool atacking = false;
    bool end = false;
    int num;
    bool reloded = true;

    Animator _animator;
    SpriteRenderer _spriteRenderer;


    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (atacking)
        {
            pointsMovements[0].transform.position = new Vector3(Player.position.x - DistancePointX, Player.position.y + DistancePointY, transform.position.z);
            pointsMovements[1].transform.position = new Vector3(Player.position.x + DistancePointX, Player.position.y + DistancePointY, transform.position.z);

            transform.position = Vector2.MoveTowards(transform.position, pointsMovements[num].position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, pointsMovements[num].position) < distanceMov)
            {
                flip();
            }

            if (Mathf.Abs(transform.position.x - Player.position.x) < 0.5)
            {
                if (reloded)
                {
                    reloded = false;
                    Invoke("Shoot", 0.25f);
                }
            }

        }

        else if (Mathf.Abs(transform.position.x - Player.position.x) < playerDetection && Mathf.Abs(transform.position.y - Player.position.y) < playerDetection)
        {
            //_animator.SetBool("atack", true);
            atacking = true;
        }
        if (end)
        {
            transform.Translate(Vector2.up * 5 * Time.deltaTime);
            Invoke("Destroy", 2f);
        }

    }

    void Shoot()
    {
        float anguloRadianes = Mathf.Atan2(Player.position.y - transform.position.y, Player.position.x - transform.position.x);
        float anguloGrados = (180 / Mathf.PI) * anguloRadianes;
        shotPoint.rotation = Quaternion.Euler(0, 0, anguloGrados);
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        reloded = true;
    }

    void flip()
    {
        if (transform.position.x < pointsMovements[num].position.x)
        {
            _spriteRenderer.flipX = true;
            num = 0;
        }
        else
        {
            _spriteRenderer.flipX = false;
            num = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetection);
    }
}
