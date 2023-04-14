using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        //�̵��ӵ�
    public float rotateSpeed = 360f;    //ȸ���ӵ�

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
        //�÷��̾��� ������Ʈ ��������
        playerInput= GetComponent<PlayerInput>();
        playerRigidbody= GetComponent<Rigidbody>();

        defaultSpeed = moveSpeed;
    }

    private void Update()
    {
        Dash();
    }

    // FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
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

        //��������� �̵��� �Ÿ� ���
        Vector3 moveDistance = moveV * defaultSpeed * Time.deltaTime;
        /*playerInput.move * transform.forward*/

        //������ٵ� ���� ���� ������Ʈ ��ġ ����
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

        transform.LookAt(transform.position + moveV);
    }

    private void Rotate()
    {
        // ��������� ȸ�� ��ġ ���
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // ������ٵ� ���� ���ӿ�����Ʈ ȸ�� ����
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
