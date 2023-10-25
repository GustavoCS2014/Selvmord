using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyJump : MonoBehaviour
{
    Rigidbody2D rb;
    Transform Player;
    [SerializeField] private Transform FloorControler;

    [SerializeField] private bool SwitchGroundCheck;
    [SerializeField] private LayerMask isGround;

    [SerializeField] float DistanceActivation = 40f;
    [SerializeField] float jumpForce = 30f;
    [SerializeField] float DistanceDetectGround = 1f;

    [SerializeField] float SpeedMin,SpeedMax;
    private float Speed;

    private float DetectionGround;
    private bool isInGround = true;

    MainSystem MS;


    private void Awake() 
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //MS = GameObject.FindGameObjectWithTag("mainSystem").GetComponent<MainSystem>();

        Speed = Random.Range(SpeedMin, SpeedMax);
        DetectionGround = Speed * 12.2f / 10f;
        FloorControler.transform.position = new Vector3(transform.position.x + DetectionGround, transform.position.y);
    }
    private void Update()
    {

        if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation)
        {
            if(isInGround && CheckGround())
            {
                Invoke("Jump", 3f);
                isInGround= false;
            }
        }
    }

    bool CheckGround()
    {
        RaycastHit2D informationFloor = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        return informationFloor;
    }

    void Jump()
    {
        if (SwitchGroundCheck)
        {
            RaycastHit2D informationFloor = Physics2D.Raycast(FloorControler.position, Vector2.down, DistanceDetectGround, isGround);

            if (!informationFloor)
            {
                Spin();
            }
            else
            {
                MovementGH();
            }

        }
        else
        {

            RaycastHit2D informationFloor = Physics2D.Raycast(transform.position, transform.right, DistanceDetectGround, isGround);

            if (informationFloor)
            {
                Spin();
            }
            else
            {
                MovementGH();
            }
        }
    }

    void Spin()
    {

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        Speed *= - 0.5f;

        DetectionGround = Speed * 12.2f / 10f;
        FloorControler.transform.position = new Vector3(transform.position.x + DetectionGround, transform.position.y);

        MovementGH();
    }

    private void SwitchData()
    {

        if (Speed < 0)
        {
            Speed = Random.Range(SpeedMin, SpeedMax);
            Speed *= -1;
        }
        else
        {
            Speed = Random.Range(SpeedMin, SpeedMax);
        }

        DetectionGround = Speed * 12.2f / 10f;
        FloorControler.transform.position = new Vector3(transform.position.x + DetectionGround, transform.position.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(FloorControler.transform.position, FloorControler.transform.position + Vector3.down * DistanceDetectGround);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);

    }

    private void MovementGH()
    {
        rb.velocity = new Vector2(Speed, rb.velocity.y);
        rb.AddForce(Vector2.up * jumpForce*100, ForceMode2D.Impulse);

        SwitchData();
        isInGround = true;
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
