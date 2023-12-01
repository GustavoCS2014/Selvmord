using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Death",5f);
    }

   void Death()
    {
        Destroy(gameObject);
    }
}
