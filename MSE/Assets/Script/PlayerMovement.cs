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
    public float moveSpeed = 5f;        //�̵��ӵ�
    public float rotateSpeed = 360f;    //ȸ���ӵ�

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
        //�÷��̾��� ������Ʈ ��������
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

    // FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
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

        //��������� �̵��� �Ÿ� ���
        Vector3 moveDistance = moveV * defaultSpeed * Time.deltaTime;

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

    //���� ������ ���ǵ带 0����
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Floor"))
        {
            defaultSpeed = 0f;
        }
    }

    //������ �������� ���ǵ� ȸ��
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
