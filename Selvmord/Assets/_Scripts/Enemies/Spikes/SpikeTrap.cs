using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    // Start is called before the first frame update

    Animator _animator;
    MainSystem MS;

    void Start()
    {
        _animator= GetComponent<Animator>();
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
    }

    // Update is called once per frame
    public void Active()
    {
        _animator.SetTrigger("Active");
    }

    public void Desactive()
    {
        _animator.SetTrigger("Desactive");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MS.DamagePlayer(30, collision.GetContact(0).normal);
        }
    }
}
