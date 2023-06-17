using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;
using static ServerManager;

public class DumyPlayer : MonoBehaviour
{
    public Animator anim;
    public WebSocketSharp.WebSocket ws = null;
    public enum Role {Cook, Hall}

    public static DumyPlayer Instance;

    public bool _handle;
    public bool _interact;
    public int hand_index;
    public GameObject[] Hands;

    public Role _role;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {

        if (_interact == true && _handle == false)
        {
            _handle = true;
            Hands[hand_index].SetActive(true);
            _interact = false;
        }
        else if(_interact == true && _handle == true)
        {
            _handle = false;
            Hands[hand_index].SetActive(false);
            _interact = false;
        }

        anim.SetBool("Stay", ServerManager.instance._receiveData._playerInfo.anim_stay);
        anim.SetBool("Handle", ServerManager.instance._receiveData._playerInfo.anim_handle);
        anim.SetBool("NoneHandle", ServerManager.instance._receiveData._playerInfo.anim_none);
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

    private void FixedUpdate()
    {
        transform.position = new Vector3(ServerManager.instance._receiveData._playerInfo.move_x
            ,transform.position.y, ServerManager.instance._receiveData._playerInfo.move_z);
        transform.rotation = Quaternion.Euler(new Vector3(0, ServerManager.instance._receiveData._playerInfo.rotation, 0));
    }

}
