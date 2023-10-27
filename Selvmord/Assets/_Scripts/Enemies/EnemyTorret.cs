using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorret : MonoBehaviour
{
    Transform Player;
    [SerializeField] bool ZoneActive;

    [SerializeField] Transform shootPoint;
    [SerializeField] Transform Body;
    [SerializeField] GameObject bullet;
    [SerializeField] float DistanceActivation;

    private bool reloded = true;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ZoneActive)
        {
            if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation && Mathf.Abs(transform.position.y - Player.position.y) < DistanceActivation)
            {
                float anguloRadianes = Mathf.Atan2(Player.position.y - transform.position.y, Player.position.x - transform.position.x);
                float anguloGrados = (180 / Mathf.PI) * anguloRadianes;
                Body.transform.rotation = Quaternion.Euler(0, 0, anguloGrados);

                if (reloded)
                {
                    reloded = false;
                    Invoke("Shoot", 2f);
                }
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        reloded = true;
    }
    private void OnDrawGizmos()
    {
        if(ZoneActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(DistanceActivation * 2, DistanceActivation * 2));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!ZoneActive)
            {
                float anguloRadianes = Mathf.Atan2(Player.position.y - transform.position.y, Player.position.x - transform.position.x);
                float anguloGrados = (180 / Mathf.PI) * anguloRadianes;
                Body.transform.rotation = Quaternion.Euler(0, 0, anguloGrados);

                if (reloded)
                {
                    reloded = false;
                    Invoke("Shoot", 2f);
                }
            }
        }
    }
}
