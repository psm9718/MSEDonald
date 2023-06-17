using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Role {Cook, Hall}

    public static Player Instance;

    public bool On_handle = false;
    public bool Is_interact = false;
    public int hand_index;
    public GameObject[] Hands;
    

    public Role _role;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        if (Is_interact == true && On_handle == false)
        {
            On_handle = true;
            Hands[hand_index].SetActive(On_handle);
            Is_interact = false;
        }
        else if(Is_interact == true && On_handle == true)
        {
            On_handle = false;
            Hands[hand_index].SetActive(On_handle);
            Is_interact = false;
        }

        DevelopChange();
    }

    //역할 바꾸기
    public void Changed()
    {
        if(this._role == Role.Cook) 
        {
            this._role = Role.Hall;
        }
        else if(this._role == Role.Hall)
        {
            this._role = Role.Cook;
        }
    }

    private void DevelopChange()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Changed();
        }
    }

    
}
