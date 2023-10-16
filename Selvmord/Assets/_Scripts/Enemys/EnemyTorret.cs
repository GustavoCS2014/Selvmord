using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorret : MonoBehaviour
{
    [SerializeField] Transform shotPoint;
    [SerializeField] Transform Player;
    [SerializeField] GameObject bullet;
    [SerializeField] float DistanceActivation;

    private bool reloded = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation)
        {
            float anguloRadianes = Mathf.Atan2(Player.position.y - transform.position.y, Player.position.x - transform.position.x);
            float anguloGrados = (180 / Mathf.PI) * anguloRadianes;
            transform.rotation = Quaternion.Euler(0, 0, anguloGrados);

            if (reloded)
            {
                reloded = false;
                Invoke("Shoot", 2f);
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        reloded = true;
    }
}
