using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public enum State {Wait, Order, Entry, PickUp, Sit, Eat, Return, Out}

    NavMeshAgent agent; // �׺���̼�
    Renderer color;
    bool isCrushed = false; // �ε��� ���� �ִ��� ������

    [Header(("Transforms"))]
    public Transform entrance; // ���� �Ա� �� �ⱸ ��ġ
    public Transform pickUpTable;
    public Transform Tables;
    public List<Transform> targets; // ���̺���� Transform�� ��Ƶδ� array
    public Transform returnPoint; // �ݳ���

    Vector3 chairPosition; // NPC�� �ڽ� ������Ʈ�� ���̺� �ɾ��ִ� ��Ҹ� ������ִ� Vector3
    int chair; // ���ʿ� ������ �����ʿ� ������
    
    [Header("NPC Info")]
    public int targetTableNum; // Ÿ�� ���̺� ��ȣ
    public float sitPositionX = 1.6f; // �÷��̾ ���� �� ���̺��� ������ �Ÿ� 
    public float sitPositionZ = 1.7f; // �÷��̾ ���� �� ���̺��� ������ �Ÿ�
    public float eatTime; // ���� �Դ� �ð�
    public float eatCycle = 5f; // �� �� ���� �Դµ� �ɸ��� �ð�

    public static NPC instance;

    public State state;
    
    void Awake()
    {
        state = State.Wait;
        color = gameObject.GetComponent<Renderer>();
        targetTableNum = this.transform.GetSiblingIndex();
        instance = this;
        for (int i = 0; i < Tables.childCount; i++)
        {
            targets.Add(Tables.GetChild(i).GetChild(0));
        }
        agent = GetComponent<NavMeshAgent>(); // �׺���̼� �ʱ�ȭ
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        // ������ 
        Moving();
    }

    // NPC ������
    void Moving()
    {
        // ������ �� ��������ٸ� �Ⱦ���� �̵� -> ������ �� ��������� state�� Order�� �ٲ㼭 �Ĵ����� �����Բ� �Ѵ�
        if (gameObject.activeSelf && (state == State.Order))
        {
            Walk();
            StartCoroutine(GotoPickUp());
            //state = State.Entry;
        }

        // �Ⱦ��뿡�� ���̺�� �̵�
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.2f && state == State.Entry){ 
            // �Ⱦ��뿡 �����ߴٸ�
            StartCoroutine(PickUp());
        }

        // ���̺�� ���� ��
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.2f && state == State.PickUp) // ���� ����
        {
            Stay();
            StartCoroutine(Sit());
        }
        
        // NPC ���°� Eat�� ��
        if (state == State.Eat)
        {
            Eat();
        }

        if(state == State.Return)
        {
            Walk();
            GoToReturnTable();
        }

        // ����
        if (state == State.Out)
        {
            agent.SetDestination(entrance.position);
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.9f && state == State.Out)
            {
                transform.position = new Vector3(26, 1, 22);
                gameObject.SetActive(false);
                Debug.Log(targetTableNum + " ���� �Ϸ�");
            }
        }
    }

    void Walk()
    {
        this.transform.GetChild(0).gameObject.GetComponent<NPCanim>().anim_stay = false;
        this.transform.GetChild(0).gameObject.GetComponent<NPCanim>().anim_none = true;
    }

    void Stay()
    {
        this.transform.GetChild(0).gameObject.GetComponent<NPCanim>().anim_stay = true;
        this.transform.GetChild(0).gameObject.GetComponent<NPCanim>().anim_none = false;
    }

    IEnumerator GotoPickUp()
    {
        yield return new WaitForSeconds(1f);
        agent.SetDestination(pickUpTable.position);
        state = State.Entry;
    }

    // �ɱ�
    IEnumerator Sit()
    {
        Debug.Log(targetTableNum + " ���̺� ����");
        state = State.Sit;
        agent.enabled = false; // �׺���̼� ��Ȱ��ȭ
        transform.rotation = Quaternion.Euler(0, 90, 0); // npc�� ���̺� ������ �ٶ󺸰�
        chair = Random.Range(0, 2);
        if(targetTableNum == 2)
        {
            sitPositionX = 1.75f; sitPositionZ = 2f;
            chairPosition = (chair == 0) ? new Vector3((-1) * sitPositionX, 0f, (-1) * sitPositionZ) : new Vector3(sitPositionX, 0f, (-1) * sitPositionZ);
        }
        else if(targetTableNum == 11 || targetTableNum == 12)
        {
            sitPositionX = 1.75f; sitPositionZ = 2f;
            chairPosition = (chair == 0) ? new Vector3((-1) * sitPositionX,0f,sitPositionZ) : new Vector3(sitPositionX,0f,sitPositionZ);
        }
        else if(targetTableNum == 0 || targetTableNum == 3 || targetTableNum == 4 || targetTableNum == 5)
        {
            sitPositionX = 2f;
            chairPosition = (chair == 0) ? new Vector3(2f, 0f, sitPositionZ * (-1)) : new Vector3(2f, 0f, sitPositionZ);
        }
        else
        {
            chairPosition = (chair == 0) ? new Vector3(sitPositionX, 0f, sitPositionZ * (-1)) : new Vector3(sitPositionX, 0f, sitPositionZ);
        }
        
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        transform.position = targets[targetTableNum].position; // npc ��ġ�� ���̺� Ÿ�� ��ġ�� ������
        if (targetTableNum == 0 || targetTableNum == 3 || targetTableNum == 4 || targetTableNum == 5) { sitPositionX = 2f; }
        transform.GetChild(0).localPosition += chairPosition; // ������ �̵�
        transform.rotation = Quaternion.Euler(0, 0, 0); // ȸ���� �ʱ�ȭ
        
        if(targetTableNum == 2 || targetTableNum == 11 || targetTableNum == 12)
        {
            transform.GetChild(0).rotation = (chair == 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 270, 0); // ���� ���� ���� ���� ȸ���� ����
        }
        else
        {
            transform.GetChild(0).rotation = (chair == 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0); // ���� ���� ���� ���� ȸ���� ����
        }
        state = State.Eat;
    }


    // �Ⱦ�
    IEnumerator PickUp()
    {
        yield return new WaitForSeconds(1f);
        agent.SetDestination(targets[targetTableNum].position); // ���̺�� �̵�
        state = State.PickUp;
    }

    // �Ļ�
    void Eat()
    {
         eatTime += Time.deltaTime; // �Ļ� �ð� Ÿ�̸�
         if (eatTime >= eatCycle) // �Ļ� �ð��� ���� �ð��� �����ϸ�
         {
             Debug.Log(targetTableNum + " ����");
             eatTime = 0;
             transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
             transform.rotation = Quaternion.Euler(0, 180, 0);
             state = State.Return;
         }
    }

    void GoToReturnTable()
    {
        transform.GetChild(0).localPosition = new Vector3(0f, -1f, 0f); // ���� ��ġ �ʱ�ȭ
        agent.enabled = true; // �׺���̼� Ȱ��ȭ
        gameObject.GetComponent<Collider>().enabled = true; // �ݶ��̴� Ȱ��ȭ
        agent.SetDestination(returnPoint.transform.position);
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.9f && state == State.Return)
        {
            Debug.Log(targetTableNum + "�ݳ� �Ϸ�");
            agent.SetDestination(entrance.position);
            StartCoroutine(Exit());
        }
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.3f);
        state = State.Out;
    }

    void OnCollisionEnter(Collision collision)
    {
        // NPC�� Player�� NPC�� �ε����� �Ѿ�����.
        if (collision.collider.gameObject.CompareTag("Player") || collision.collider.gameObject.CompareTag("Npc"))
        {
            StartCoroutine(Fall());
        }
    }

    // �Ѿ����� ȿ���� ����������
    IEnumerator Fall()
    {
        Debug.Log("Ouch! Stop!");
        if (!isCrushed)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            color.material.color += new UnityEngine.Color(0f, 0f, 0f, 0.5f);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            isCrushed = true;
            yield return new WaitForSeconds(2f);
            gameObject.GetComponent<Collider>().enabled = true;
            color.material.color -= new UnityEngine.Color(0f, 0f, 0f, 0.5f);
            agent.isStopped = false;
            agent.speed = 6f;
            agent.SetDestination(entrance.position);
            state = State.Out;
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }

    // ��� ��ȣ
    public void GoToPickUp()
    {
        state = State.Order;
    }
}