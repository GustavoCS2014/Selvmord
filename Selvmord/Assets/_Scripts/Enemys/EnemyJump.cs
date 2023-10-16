using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    [SerializeField] private bool CheckFoor;
    [SerializeField] private LayerMask isFloor;

    Rigidbody2D rb;
    Transform Player;
    [SerializeField] float DistanceActivation = 40f;

    [SerializeField] float speed;
    [SerializeField] Transform FloorControler;
    [SerializeField] float distance;
    [SerializeField] bool direction;

    [SerializeField] float jumpForce;
    private bool jump = true;
    private bool spin = true;

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

                RaycastHit2D informationFloor = Physics2D.Raycast(FloorControler.position, Vector2.down, distance);

                if (jump)
                {
                    jump = false;
                    Invoke("MovementGH", 3f);

                }

                if (informationFloor == false)
                {
                    if (spin)
                    {
                        spin = false;
                        Invoke("Spin", 1f);
                    }

                }

            }
            else
            {
                if (jump)
                {
                    jump = false;
                    Invoke("MovementGH", 3f);

                }

                RaycastHit2D informationFloor = Physics2D.Raycast(transform.position, transform.up, distance, isFloor);

                if (informationFloor)
                {
                    if (spin)
                    {
                        spin = false;
                        Invoke("Spin", 1f);
                    }
                }
            }
        }
       
    }

    void Spin()
    {
        direction = !direction;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 90);
        speed *= -1;
        spin = true;
    }
    private void OnDrawGizmos()
    {
        if (CheckFoor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(FloorControler.transform.position, FloorControler.transform.position + Vector3.down * distance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * distance);
        }
       
    }

    private void MovementGH()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetContact(0).normal.y <= -0.9)
            {
                collision.gameObject.GetComponent<PlayerMovement>().ReboundPlayer();
                gameObject.SetActive(false);
                MS.AddSoul(1);
            }
            else
            {
                MS.DamagePlayer(30, collision.GetContact(0).normal);
            }
        }
    }
}
