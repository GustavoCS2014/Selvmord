using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    [SerializeField] bool ActiveDoor;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!ActiveDoor)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerPrefs.SetInt("BossFight", 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("BossFight", 0);
            CameraController.StartBossFight = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("BossFight", 0);
        }
    }
}
