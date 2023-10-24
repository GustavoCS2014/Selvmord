using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnControler : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform Player;

    private bool Spawn = true;

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - Player.position.x) < 20)
        {
            if (Spawn)
            {
                Instantiate(enemy);
                Spawn = false;
            }
           
        } 
        
    }
}
