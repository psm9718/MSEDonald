using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Out : MonoBehaviour
{ 
    public bool Is_interact = false;

    // Update is called once per frame
    void Update()
    {
        if (Is_interact)
        {
            Gamemanager.instance.point += 50;
            Is_interact = false;
        }
    }
}
