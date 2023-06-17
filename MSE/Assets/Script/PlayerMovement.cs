using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    

    [System.Serializable]
    private class PlayerMove
    {
        public int On_handle;
        public int Is_interact;
        public int hand_index;
    }

    public Animator anim;
    public float moveSpeed = 5f;        //이동속도
    public float rotateSpeed = 360f;    //회전속도

    public bool isDash = false;
    public float defaultSpeed = 0f;
    public float dashSpeed = 10f;
    public float DashdueTime = 0f;
    public float dashTime = 0f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    public ServerManager.GameData _sendData;

    RaycastHit hit;
    Vector3 moveV;
    
    void Start()
    {
        //플레이어의 컴포넌트 가져오기
        playerInput= GetComponent<PlayerInput>();
        playerRigidbody= GetComponent<Rigidbody>();

        defaultSpeed = moveSpeed;
    }

    private void Update()
    {
        Dash();
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("Stay", false);
        }
        else
        {
            anim.SetBool("Stay", true);
            anim.SetBool("Handle", false);
            anim.SetBool("NoneHandle", false);
        }
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        Move();
        Packaging();
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Player.Instance.On_handle)
            {
                anim.SetBool("Handle", true);
            }
            else
            {
                anim.SetBool("NoneHandle", true);
            }
        }
        
    }


    private void newMove()
    {
        moveV = new Vector3(-playerInput.move, 0, playerInput.rotate).normalized;

        transform.position += moveV * defaultSpeed * Time.deltaTime;

        transform.LookAt(transform.position + moveV);
    }

    private void Move()
    {

        moveV = new Vector3(-playerInput.move, 0, playerInput.rotate).normalized;

        //상대적으로 이동할 거리 계산
        Vector3 moveDistance = moveV * defaultSpeed * Time.deltaTime;

        //리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

        transform.LookAt(transform.position + moveV);
    }

    private void Rotate()
    {
        // 상대적으로 회전 수치 계산
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // 리지드바디를 통해 게임오브젝트 회전 변경
        playerRigidbody.rotation =
            playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);
        
    }

    private void Dash()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            isDash = true;
        }
        if(dashTime <= 0)
        {
            defaultSpeed = moveSpeed;
            if(isDash)
            {
                dashTime = DashdueTime;
            }
        }
        else
        {
            dashTime -= Time.deltaTime;

            if (Physics.Raycast(this.transform.position + new Vector3(0f, 0.4f, 0f), this.transform.forward, out hit, 0.7f))
            {
                Debug.DrawRay(this.transform.position + new Vector3(0f, 0.4f, 0f), this.transform.forward * hit.distance, Color.blue);
                defaultSpeed = 0f;
            }
            else
            {
                defaultSpeed = dashSpeed;
            }
        }
        isDash = false;
    }

    //벽에 닿으면 스피드를 0으로
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Floor"))
        {
            defaultSpeed = 0f;
        }
    }

    //벽에서 떨어지면 스피드 회복
    private void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.CompareTag("Floor"))
        {
            defaultSpeed = moveSpeed;
        }
    }

    private void Packaging()
    {

        _sendData._playerInfo.move_x = this.transform.position.x;
        _sendData._playerInfo.move_z = this.transform.position.z;
        _sendData._playerInfo.rotation = this.transform.rotation.eulerAngles.y;
        _sendData._playerInfo.anim_stay = anim.GetBool("Stay");
        _sendData._playerInfo.anim_none = anim.GetBool("NoneHandle");
        _sendData._playerInfo.anim_handle = anim.GetBool("Handle");


        ServerManager.instance._gameData._playerInfo = _sendData._playerInfo;
    }
}
