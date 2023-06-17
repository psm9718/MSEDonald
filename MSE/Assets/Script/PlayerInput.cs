using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";

    public float move { get; private set; }
    public float rotate { get; private set; }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            move = Input.GetAxis(moveAxisName);
            rotate = Input.GetAxis(rotateAxisName);
        }
        else
        {
            move = 0;
            rotate = 0;
        }
            
    }
}
