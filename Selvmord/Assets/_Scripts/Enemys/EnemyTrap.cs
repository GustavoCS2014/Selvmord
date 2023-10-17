using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] bool ZoneActive;

    private Transform Player;
    private bool reloded = true;

    [SerializeField] float DistanceActivation;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform [] ShootPoint;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation)
        {
            if (reloded)
            {
                reloded = false;
                Invoke("Shoot", 1.5f);
            }
        }
    }

    void Shoot()
    {
        for (int i = 0; i < ShootPoint.Length ; i++)
        {
            Instantiate(bullet, ShootPoint[i].position, ShootPoint[0].rotation);
        }
        reloded = true;
    }

    private void OnDrawGizmos()
    {
        if (ZoneActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(DistanceActivation * 2, DistanceActivation * 1.5f));
        }
    }
}
