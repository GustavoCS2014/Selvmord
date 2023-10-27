using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Transform StartPoint;
    Transform EndPoint;
    private bool reloded = true;

    [SerializeField] Transform[] points;
    [SerializeField] GameObject bullet;
    public static float VelocityMovementBoss = 5f;
    public static bool StartBossFight = false;

    // Start is called before the first frame update
    void Start()
    {
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        EndPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(StartBossFight)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(EndPoint.position.x,EndPoint.position.y+15), VelocityMovementBoss * Time.deltaTime);
            if (reloded)
            {
                Invoke("Shoot", 1f);
                reloded = false;
            }
        }
        else
        {
            transform.position = new Vector3(StartPoint.position.x, StartPoint.position.y + 14f);
        }
    }

    private void Shoot()
    {
        int random = Random.Range(0, points.Length);
        Instantiate(bullet, points[random].position, points[0].rotation);    
        reloded= true;
    }
}
