using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumyTargettingRaycast : MonoBehaviour
{
    RaycastHit hit;

    private ServerManager.PlayerInfo _otherInfo;
    private bool _innerDone;
    void Update()
    {
        _otherInfo = ServerManager.instance._receiveData._playerInfo;

        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f))
        {
            // ���� ��Ʈ��Ű�� ��������
            if (ServerManager.instance._receiveData._leftcontrol)
            {
                if (!_innerDone)
                {
                    if(Player.Instance._role == Player.Role.Cook)
                    {
                        _HallAct();
                    }
                    else if(Player.Instance._role == Player.Role.Hall)
                    {
                        _CookAct();
                    }
                    
                    if (hit.collider.tag == "Trash")
                    {
                        Player.Instance.Is_interact = true;
                        Player.Instance.On_handle = true;
                    }
                    
                    _innerDone = true;
                }
                else
                {
                    ServerManager.instance._gameData._leftcontrolDone = true;
                }
                
            }
            else
            {
                _innerDone = false;
                ServerManager.instance._gameData._leftcontrolDone = false;
            }
            
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
        }
    }

    // cook�϶� �ϴ� �ൿ
    void _CookAct()
    {
        if (hit.collider.tag == "Soda")
        {
            hit.collider.GetComponent<Food>().ontarget = true;
            DumyPlayer.Instance._interact = true;
            DumyPlayer.Instance.hand_index = 0;
        }
        else if (hit.collider.tag == "Fried")
        {
            hit.collider.GetComponent<Food>().ontarget = true;
            DumyPlayer.Instance._interact = true;
            DumyPlayer.Instance.hand_index = 1;
        }
        else if (hit.collider.tag == "Burger")
        {
            hit.collider.GetComponent<Food>().ontarget = true;
            DumyPlayer.Instance._interact = true;
            DumyPlayer.Instance.hand_index = 2;
        }
        else if (hit.collider.tag == "SodaMacine")
        {
            hit.collider.GetComponent<Sodamacine>().Is_interact = true;

        }
        else if (hit.collider.tag == "Food_Out")
        {
            if (DumyPlayer.Instance._handle)
            {
                DumyPlayer.Instance._interact = true;
                hit.collider.GetComponent<Food_Out>().Is_interact = true;
            }
        }
    }

    // hall�϶� �ϴ� �ൿ
    void _HallAct()
    {
        if (hit.collider.tag == "Clean")
        {
            DumyPlayer.Instance._interact = true;
            DumyPlayer.Instance.hand_index = 3;
        }
        else if (hit.collider.tag == "Dirty")
        {
            hit.collider.GetComponent<Dirty>().Is_interact = true;

        }
    }
}
