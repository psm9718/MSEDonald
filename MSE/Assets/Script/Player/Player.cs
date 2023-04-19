using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public bool On_handle = false;
    public bool Is_interact = false;
    public int food_index;
    public GameObject[] Foods;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Is_interact == true && On_handle == false)
        {
            On_handle = true;
            Foods[food_index].SetActive(On_handle);
            Is_interact = false;
        }
        else if(Is_interact == true && On_handle == true)
        {
            On_handle = false;
            Foods[food_index].SetActive(On_handle);
            Is_interact = false;
        }
    }
}
