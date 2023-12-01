using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    Transform Player;

    [SerializeField] GameObject ParticulaSouls;

    [SerializeField] private bool SeeActiveDistance,SwitchGroundCheck;
    [SerializeField] float DistanceActivation = 30f;
    private Rigidbody2D rb;

    [SerializeField] private float SpeedMin,SpeedMax;
    private float speed;

    [SerializeField] private float WaitTimeMin, WaitTimeMax;
    private float WaitTime;

    [SerializeField] private float distance;
    [SerializeField] private Transform GroundCheck,StopPoint;
    [SerializeField] private LayerMask isGround;

    private bool Wait = false;

    MainSystem MS;
    Animator _animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
        _animator= GetComponent<Animator>();

        speed = Random.Range(SpeedMin, SpeedMax);
        WaitTime = Random.Range(WaitTimeMin, WaitTimeMax);

    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
       

        if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation)
        {
            _animator.SetBool("Waiting",Wait);

            if (SwitchGroundCheck)
            {
                if(!Wait)
                {
                    rb.velocity = new Vector2(speed * transform.right.x, rb.velocity.y);

                    RaycastHit2D GroundStop = Physics2D.Raycast(GroundCheck.position, Vector2.down, distance);

                    if (!GroundStop)
                    {
                        speed -= (Time.deltaTime * speed);
                    }

                    RaycastHit2D informationFloor = Physics2D.Raycast(StopPoint.position, Vector2.down, distance);

                    if (!informationFloor)
                    {
                        Stop();
                    }

                    if(speed < 1)
                    {
                        Stop();
                    }
                }
            }
            else
            {
                if (!Wait)
                {
                    rb.velocity = new Vector2(speed * transform.right.x, rb.velocity.y);

                    RaycastHit2D informationFloor = Physics2D.Raycast(transform.position, transform.right, distance, isGround);

                    if (informationFloor)
                    {
                        Stop();
                    }
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    void Stop()
    {
        rb.velocity = new Vector2(0, 0);
        Wait = true;
        Invoke("Spin", WaitTime);
    }

    void Spin()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        speed = Random.Range(SpeedMin, SpeedMax);
        WaitTime = Random.Range(WaitTimeMin, WaitTimeMax);
        Wait = false;
    }

    private void OnDrawGizmos()
    {
        if (SwitchGroundCheck)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(GroundCheck.transform.position, GroundCheck.transform.position + Vector3.down * distance);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * distance);
        }

        if (SeeActiveDistance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(DistanceActivation * 2, DistanceActivation * 1.5f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetContact(0).normal.y <= -0.9)
            {
                collision.gameObject.GetComponent<PlayerMovement>().ReboundPlayer();
                Instantiate(ParticulaSouls, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
            else
            {
                MS.DamagePlayer(20, collision.GetContact(0).normal);
            }

        }
    }
}
