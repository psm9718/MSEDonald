using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingRaycast : MonoBehaviour
{
    RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f))
        {
            // 왼쪽 컨트롤키를 눌렀을때
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ServerManager.instance._gameData._leftcontrol = true;

                if (Player.Instance._role == Player.Role.Cook)
                {
                    _CookAct();
                }
                else if(Player.Instance._role == Player.Role.Hall)
                {
                    _HallAct();
                }

                if (hit.collider.tag == "Trash")
                {
                    Player.Instance.Is_interact = true;
                    Player.Instance.On_handle = true;
                }
            }

            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
        }
        
        if(ServerManager.instance._receiveData._leftcontrolDone)
        {
            ServerManager.instance._gameData._leftcontrol = false;
        }
    }

    // cook일때 하는 행동
    void _CookAct()
    {
        if (hit.collider.tag == "Soda")
        {
            hit.collider.GetComponent<Food>().ontarget = true;
            Player.Instance.Is_interact = true;
            Player.Instance.hand_index = 0;
        }
        else if (hit.collider.tag == "Fried")
        {
            hit.collider.GetComponent<Food>().ontarget = true;
            Player.Instance.Is_interact = true;
            Player.Instance.hand_index = 1;
        }
        else if (hit.collider.tag == "Burger")
        {
            hit.collider.GetComponent<Food>().ontarget = true;
            Player.Instance.Is_interact = true;
            Player.Instance.hand_index = 2;
        }
        else if (hit.collider.tag == "SodaMacine")
        {
            hit.collider.GetComponent<Sodamacine>().Is_interact = true;

        }
        else if (hit.collider.tag == "Food_Out")
        {
            if (Player.Instance.On_handle)
            {
                Player.Instance.Is_interact = true;
                hit.collider.GetComponent<Food_Out>().Is_interact = true;
            }
        }
    }

    // hall일때 하는 행동
    void _HallAct()
    {
        if (hit.collider.tag == "Clean")
        {
            Player.Instance.Is_interact = true;
            Player.Instance.hand_index = 3;
        }
        else if (hit.collider.tag == "Dirty" && Player.Instance.On_handle)
        {
            hit.collider.GetComponent<Dirty>().Is_interact = true;

        }
    }
}
