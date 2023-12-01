using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    [SerializeField] bool ActiveDoor;

    bool TargetCamara = false;
    CameraController CC;

    private void Start()
    {
        PlayerPrefs.SetInt("BossFight", 0);
        Boss.StartBossFight = false;
        CC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!ActiveDoor)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerPrefs.SetInt("BossFight", 1);
                TargetCamara = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt("BossFight", 0);
            Boss.StartBossFight = false;
            
            if(!TargetCamara) 
            {
                TargetCamara= true;
                CC.ReturnPlayerPosition();
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("BossFight", 0);
            Boss.StartBossFight = false;
        }
    }
}
