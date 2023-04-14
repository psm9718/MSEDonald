using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        //이동속도
    public float rotateSpeed = 360f;    //회전속도

    public bool isDash = false;
    public float defaultSpeed = 0f;
    public float dashSpeed = 10f;
    public float DashdueTime = 0f;
    public float dashTime = 0f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    Vector3 moveV;
    // Start is called before the first frame update
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
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        //newMove();
        //Rotate();
        Move();
        //Dash();
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
        /*playerInput.move * transform.forward*/

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
            defaultSpeed = dashSpeed;
        }
        isDash = false;
    }
}
