using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public static NPC Instance;

    NavMeshAgent agent; // �׺���̼�

    public Transform entrance; // ���� �Ա� �� �ⱸ ��ġ
    public Transform[] tables; // ���̺���� Transform�� ��Ƶδ� array
    public Transform NPCGround; // NPC���� ��Ƶ� ��
    public int targetTableNum; // Ÿ�� ���̺� ��ȣ
    Vector3 chairPosition; // NPC�� �ڽ� ������Ʈ�� ���̺� �ɾ��ִ� ��Ҹ� ������ִ� Vector3
    int chair; // ���ʿ� ������ �����ʿ� ������

    public float sitPosition = 1.7f; // �÷��̾ ���� �� ���̺��� ������ �Ÿ�
    public bool isEntry = true; // ���� ���̺��� ���� �����ִ� ������ Ȯ��
    public bool isServe; // ������ ���Դ��� �ȳ��Դ���
    public bool isExit; // ������ �� �Ծ����� �ƴ���
    public float eatTime; // ���� �Դ� �ð�
    public float eatCycle = 5f; // �� �� ���� �Դµ� �ɸ��� �ð�

    public List<int> orderMenu = new List<int>(); // �ֹ��� �޴��� ��� List. 0 = �ܹ���, 1 = ����Ƣ��, 2 = ��ī�ݶ�

    void Awake()
    {
        this.gameObject.SetActive(false);
        agent = GetComponent<NavMeshAgent>(); // �׺���̼� �ʱ�ȭ
    }

    void Update()
    {
        // ������ 
        Moving();
        // �Ա�
        Eat();
    }

    // NPC ������
    void Moving()
    {
        // ����
        if (gameObject.activeSelf && isEntry)
        {
            agent.SetDestination(tables[targetTableNum].position); // ���̺�� �̵�
        }

        // ���̺�� ���� ��
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.2f && isEntry) // ���� ����
        {
            Debug.Log(targetTableNum + " ���̺� ����");
            agent.enabled = false; // �׺���̼� ��Ȱ��ȭ
            transform.rotation = Quaternion.Euler(0, 90, 0); // npc�� ���̺� ������ �ٶ󺸰�
            sitDownTable();
            Order();
        }

        // ����
        if (isExit)
        {
            transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f); // ���� ��ġ �ʱ�ȭ
            agent.enabled = true; // �׺���̼� Ȱ��ȭ
            StartCoroutine(Exit());
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.9f)
            {
                transform.position = NPCGround.transform.position;
                gameObject.SetActive(false);
                Final();
            }
        }
    }

    // ���̺� �ɱ�
    void sitDownTable()
    {
        chair = Random.Range(0, 2);
        chairPosition = (chair == 0) ? new Vector3(0f, 0f, sitPosition * (-1)) : new Vector3(0f, 0f, sitPosition);
        StartCoroutine(Sit());
        isEntry = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // �ε巯�� �������� ���� �ð� ������
    IEnumerator Sit()
    {
        yield return new WaitForSeconds(0.3f);
        transform.position = tables[targetTableNum].position; // npc ��ġ�� ���̺� Ÿ�� ��ġ�� ������
        transform.GetChild(0).localPosition += (new Vector3(1f, 0f, 0f) + chairPosition); // ������ �̵�
        transform.rotation = Quaternion.Euler(0, 0, 0); // ȸ���� �ʱ�ȭ
        transform.GetChild(0).rotation = (chair == 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0); // ���� ���� ���� ���� ȸ���� ����
    }

    // ����
    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Collider>().enabled = true;
        agent.SetDestination(entrance.position);
    }

    // �޴� �ֹ�
    void Order()
    {
        orderMenu.Add(0); // �ܹ��Ŵ� ������ �ֹ�
        int ran1, ran2;
        ran1 = Random.Range(0, 2);
        ran2 = Random.Range(0, 2);
        if(ran1 == 1) orderMenu.Add(1); // 2���� 1 Ȯ���� ����Ƣ�� �ֹ�
        if (ran2 == 1) orderMenu.Add(2); // 2���� 1 Ȯ���� ����� �ֹ�

        StartCoroutine(Serve());
    }

    // ���� �ϷḦ Ȯ���ϱ� ���� �׽�Ʈ �ڵ�
    IEnumerator Serve()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log(targetTableNum + " ���� �Ϸ�");
        isServe = true;
    }

    // �Ļ�
    void Eat()
    {
        if (isServe) // ������ �ƴٸ�
        {
            eatTime += Time.deltaTime; // �Ļ� �ð� Ÿ�̸�
            if(eatTime >= eatCycle) // �Ļ� �ð��� ���� �ð��� �����ϸ�
            {
                Debug.Log(targetTableNum + " ����");
                eatTime = 0; 
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Euler(0,180,0);
                isExit = true;
                isServe = false;
            }
        }
    }

    // �Ա��� ���� ��, ���� ������ �ʱ�ȭ
    void Final() 
    {
        Debug.Log(targetTableNum + " ���� �Ϸ�");
        isEntry = !isEntry; isExit = !isExit;
        orderMenu.Clear();
    }
}