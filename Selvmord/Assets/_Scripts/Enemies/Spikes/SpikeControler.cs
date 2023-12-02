using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeControler : MonoBehaviour
{
    [SerializeField] bool ZoneActive;
    Transform Player;
    [SerializeField] SpikeTrap[] ST;
    [SerializeField] float DistanceActivation;
    [SerializeField] float TimeCoolDown;
    [SerializeField] float TimeStay;
    [SerializeField] float timeBetweenSpikes;

    private bool Active = false;

    private void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - Player.position.x) < DistanceActivation && Mathf.Abs(transform.position.y - Player.position.y) < DistanceActivation)
        {
            if (!Active)
            {
                StartCoroutine(ActiveSpikes());
                Active= true;
            }

        }
    }

    private IEnumerator ActiveSpikes()
    {
        StartCoroutine(StartActive());
        yield return new WaitForSeconds(TimeStay);
        StartCoroutine(StartDesactive());
        Invoke("Reloded",TimeCoolDown);
    }

    private IEnumerator StartActive()
    {
        for(int i = 0; i < ST.Length; i++)
        {
            ST[i].Active();
            yield return new WaitForSeconds(timeBetweenSpikes);
        }
    }

    private IEnumerator StartDesactive()
    {
        for (int i = 0; i < ST.Length; i++)
        {
            ST[i].Desactive();
            yield return new WaitForSeconds(timeBetweenSpikes);
        }
    }

    void Reloded()
    {
        Active = false;
    }

    private void OnDrawGizmos()
    {
        if (ZoneActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(DistanceActivation * 2, DistanceActivation * 2));
        }
    }
}
