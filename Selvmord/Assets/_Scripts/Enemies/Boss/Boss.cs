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

    private float CurrentTime;

    [SerializeField] Transform[] HeavyPoints;
    [SerializeField] GameObject Warning;

    [SerializeField] GameObject HeavyBullet;
    [SerializeField] GameObject HeavyBulletFinal;

    public static float VelocityMovementBoss = 3f;
    public static bool StartBossFight = false;

    bool Reset = false;
    public static bool FinalAtack = false;

    Animator _animator;
    private void Start()
    {
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        EndPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (FinalAtack)
        {

            if (!Reset)
            {
                Reset = true;
                HeavyBullet.SetActive(false);
                Warning.SetActive(false);
                _animator.SetBool("Atacking", false);
                StartCoroutine(HeavyAtackEnd());
            }

        } else if (StartBossFight)
        {
            if (Reset)
            {
                StopAllCoroutines();
                FinalAtack = false;
                Reset = false;
                reloded = true;
                HeavyAtack = true;
            }

            transform.position = Vector2.MoveTowards(transform.position, new Vector3(EndPoint.position.x,EndPoint.position.y+14.5f), VelocityMovementBoss * Time.deltaTime);
            if (reloded)
            {
                Invoke("Shoot", TimeShoot);
                reloded = false;
            }

            if(HeavyAtack)
            {
                StartCoroutine(HeavyAtackWarning());
                HeavyAtack= false;
            }

            if (transform.position == new Vector3(EndPoint.position.x, EndPoint.position.y + 14.5f)) FinalAtack = true;
        }
        else
        {
            if (!Reset)
            {
                transform.position = new Vector3(StartPoint.position.x, StartPoint.position.y + 14.5f);
                HeavyBullet.SetActive(false);
                Warning.SetActive(false);
                _animator.SetBool("Atacking", false);
                HeavyAtack = false;
                Reset = true;
                StopAllCoroutines();
            }
            
        }


    }

    private void Shoot()
    {
        if (Reset) return;
        int random = Random.Range(0, points.Length);
        Instantiate(bullet, new Vector3(points[random].transform.position.x, points[random].transform.position.y+3,0), points[0].rotation);    
        reloded= true;
    }

    private IEnumerator HeavyAtackWarning()
    {
        int random = Random.Range(0, HeavyPoints.Length);
        EmergencyNotice(random);
        yield return new WaitForSeconds(TimeWarning);
        Warning.SetActive(false);
        if (!Reset) Atack(random);
        Invoke("RelodedHeavyAttack", TimeHeavyAttack);
    }
    private IEnumerator HeavyAtackEnd()
    {
        for(int i = 0 ; i < points.Length; i += 3) 
        {
            EmergencyNotice(i);
            yield return new WaitForSeconds(.5f);
            Warning.SetActive(false);
            InstanceAtack(i);
        }
        FinalAtack = false;
    }

    void EmergencyNotice(int random)
    {
        if (Reset) return;
        Warning.SetActive(true);
        Warning.transform.position = new Vector3(HeavyPoints[random].transform.position.x, HeavyPoints[random].transform.position.y,0);
    }

    void Atack(int random)
    {
        if (Reset) return;
        HeavyBullet.SetActive(true);
        _animator.SetBool("Atacking",true);
        HeavyBullet.transform.position = new Vector3(HeavyPoints[random].transform.position.x, HeavyBullet.transform.position.y, 0);
    }

    void InstanceAtack(int random)
    {
        Instantiate(HeavyBulletFinal, new Vector3(points[random].transform.position.x, HeavyBullet.transform.position.y, 0), HeavyPoints[2].rotation);
        _animator.SetBool("Atacking", true);
    }

    void RelodedHeavyAttack()
    {
        if (Reset) return;
        HeavyBullet.SetActive(false);
        _animator.SetBool("Atacking", false);
        Invoke("RelodedAtack", TimeHeavyAttack / 2);
    }

    void RelodedAtack()
    {
        if (Reset) return;
        HeavyAtack = true;
    }
}
