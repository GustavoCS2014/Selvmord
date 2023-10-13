using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExtraJump : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private CircleCollider2D circlecCollider;
    [SerializeField] private Light2D light;


    private void OnEnable()
    {
        EventManager.Instance.OnWorldFall += ActivateConsumable;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnWorldFall -= ActivateConsumable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.ExtraJumpCollected(); //? Calls the event in the ConsumableEvents script
            sprite.enabled = false;
            circlecCollider.enabled = false;
            light.enabled = false;

        }
    }

    private void ActivateConsumable()
    {
        sprite.enabled = true;
        circlecCollider.enabled = true;
        light.enabled = true;
    }
}
