using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCamara : MonoBehaviour
{

    private bool TargetCamara = false;
    CameraController CC;
    // Start is called before the first frame update
    void Start()
    {
        CC= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!TargetCamara)
        {
            TargetCamara = true;
            CC.ReturnPlayerPosition();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TargetCamara = false;
        }
    }
}
