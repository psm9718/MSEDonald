using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soda : MonoBehaviour
{
    public bool ontarget = false;
    // Update is called once per frame
    void Update()
    {
        if (ontarget)
        {
            ontarget = false;
            Destroy(gameObject);
        }
    }
}
