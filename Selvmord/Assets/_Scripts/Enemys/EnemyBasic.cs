using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField] private bool CheckFoor;

    Transform Player;
    [SerializeField] float DistanceActivation = 30f;
    private Rigidbody2D rb;


    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private Transform Floor;
    [SerializeField] private LayerMask isFloor;
    MainSystem MS;

    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MS = GameObject.FindGameObjectWithTag("mainSystem").GetComponent<MainSystem>();
    }
    private void Update()
    {
        if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation)
        {
            if (CheckFoor)
            {
                rb.velocity = new Vector2(speed * transform.right.x, rb.velocity.y);
                RaycastHit2D informationFloor = Physics2D.Raycast(Floor.position, Vector2.down, distance);

                if (!informationFloor)
                {
                    Spin();
                }
            }
            else
            {
                rb.velocity = new Vector2(speed * transform.right.x, rb.velocity.y);

                RaycastHit2D informationFloor = Physics2D.Raycast(transform.position, transform.right, distance, isFloor);

                if (informationFloor)
                {
                    Spin();
                }
            }
        }
    }

    void Spin()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        if (CheckFoor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Floor.transform.position, Floor.transform.position + Vector3.down * distance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * distance);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetContact(0).normal.y <= -0.9)
            {
                collision.gameObject.GetComponent<PlayerMovement>().ReboundPlayer();
                MS.AddSoul(1);
                gameObject.SetActive(false);
            }
            else
            {
                MS.DamagePlayer(30, collision.GetContact(0).normal);
            }

        }
    }
}
