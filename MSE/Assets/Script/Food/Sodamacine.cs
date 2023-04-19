using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sodamacine : MonoBehaviour
{
    public Transform[] Spwanpoints;
    public bool Is_interact = false;
    [SerializeField]
    private GameObject food;
    void Update()
    {
        if (Is_interact)
        {
            Instantiate(food, Spwanpoints[1].position - new Vector3(0,1.0f,0), food.transform.rotation);
            Is_interact=false;
        }
    }
}
