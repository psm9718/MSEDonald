using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool ontarget = false;
    
    void Update()
    {
        if (ontarget)
        {
            ontarget = false;
            Destroy(gameObject);
        }
    }
}
