using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Transform StartPoint;
    Transform EndPoint;
    
    private bool reloded = true;
    [SerializeField] private float TimeShoot;
    [SerializeField] Transform[] points;

    [SerializeField] GameObject bullet;

    private bool HeavyAtack = true;
    [SerializeField] private float TimeHeavyAttack;
    [SerializeField] private float TimeWarning;
    [SerializeField] Transform[] HeavyPoints;
    [SerializeField] GameObject Warning;

    [SerializeField] GameObject HeavyBullet;

    public static float VelocityMovementBoss = 3f;
    public static bool StartBossFight = false;

    private void Start()
    {
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        EndPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
    }

    // Update is called once per frame
    void Update()
    { 
        if(StartBossFight)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(EndPoint.position.x,EndPoint.position.y+12f), VelocityMovementBoss * Time.deltaTime);
            if (reloded)
            {
                Invoke("Shoot", 1f);
                reloded = false;
            }

            if(HeavyAtack)
            {
                StartCoroutine(HeavyAtackWarning());
                HeavyAtack= false;
            }
        }
        else
        {
            transform.position = new Vector3(StartPoint.position.x, StartPoint.position.y + 12f);
        }
    }

    private void Shoot()
    {
        int random = Random.Range(0, points.Length);
        Instantiate(bullet, points[random].position, points[0].rotation);    
        reloded= true;
    }

    private IEnumerator HeavyAtackWarning()
    {
        int random = Random.Range(0, HeavyPoints.Length);
        
        EmergencyNotice(random);
        yield return new WaitForSeconds(TimeWarning);
        Warning.SetActive(false);
        Atack(random);
        Invoke("RelodedHeavyAttack", TimeHeavyAttack);
    }

    void EmergencyNotice(int random)
    {
        Warning.SetActive(true);
        Warning.transform.position = HeavyPoints[random].transform.position;
    }

    void Atack(int random)
    {
        Instantiate(HeavyBullet, points[random].position, points[1].rotation);
    }

    void RelodedHeavyAttack()
    {
        HeavyAtack= true;
    }
}
