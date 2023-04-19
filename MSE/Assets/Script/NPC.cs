using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public static NPC Instance;

    NavMeshAgent agent; // 네비게이션

    public Transform entrance; // 가게 입구 및 출구 위치
    public Transform[] tables; // 테이블들의 Transform을 담아두는 array
    public Transform NPCGround; // NPC들을 모아둔 곳
    public int targetTableNum; // 타겟 테이블 번호
    Vector3 chairPosition; // NPC의 자식 오브젝트가 테이블에 앉아있는 장소를 계산해주는 Vector3
    int chair; // 왼쪽에 앉을지 오른쪽에 앉을지

    public float sitPosition = 1.7f; // 플레이어가 앉을 때 테이블에서 떨어진 거리
    public bool isEntry = true; // 현재 테이블을 향해 가고있는 중인지 확인
    public bool isServe; // 음식이 나왔는지 안나왔는지
    public bool isExit; // 음식을 다 먹었는지 아닌지
    public float eatTime; // 음식 먹는 시간
    public float eatCycle = 5f; // 한 번 음식 먹는데 걸리는 시간

    public List<int> orderMenu = new List<int>(); // 주문한 메뉴를 담는 List. 0 = 햄버거, 1 = 감자튀김, 2 = 코카콜라

    void Awake()
    {
        this.gameObject.SetActive(false);
        agent = GetComponent<NavMeshAgent>(); // 네비게이션 초기화
    }

    void Update()
    {
        // 움직임 
        Moving();
        // 먹기
        Eat();
    }

    // NPC 움직임
    void Moving()
    {
        // 입장
        if (gameObject.activeSelf && isEntry)
        {
            agent.SetDestination(tables[targetTableNum].position); // 테이블로 이동
        }

        // 테이블로 도착 후
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.2f && isEntry) // 도착 판정
        {
            Debug.Log(targetTableNum + " 테이블 도착");
            agent.enabled = false; // 네비게이션 비활성화
            transform.rotation = Quaternion.Euler(0, 90, 0); // npc가 테이블 방향을 바라보게
            sitDownTable();
            Order();
        }

        // 퇴장
        if (isExit)
        {
            transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f); // 외형 위치 초기화
            agent.enabled = true; // 네비게이션 활성화
            StartCoroutine(Exit());
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.9f)
            {
                transform.position = NPCGround.transform.position;
                gameObject.SetActive(false);
                Final();
            }
        }
    }

    // 테이블에 앉기
    void sitDownTable()
    {
        chair = Random.Range(0, 2);
        chairPosition = (chair == 0) ? new Vector3(0f, 0f, sitPosition * (-1)) : new Vector3(0f, 0f, sitPosition);
        StartCoroutine(Sit());
        isEntry = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // 부드러운 움직임을 위한 시간 딜레이
    IEnumerator Sit()
    {
        yield return new WaitForSeconds(0.3f);
        transform.position = tables[targetTableNum].position; // npc 위치를 테이블 타겟 위치로 재조정
        transform.GetChild(0).localPosition += (new Vector3(1f, 0f, 0f) + chairPosition); // 외형만 이동
        transform.rotation = Quaternion.Euler(0, 0, 0); // 회전값 초기화
        transform.GetChild(0).rotation = (chair == 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0); // 앉은 곳에 따라 외형 회전값 조정
    }

    // 퇴장
    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Collider>().enabled = true;
        agent.SetDestination(entrance.position);
    }

    // 메뉴 주문
    void Order()
    {
        orderMenu.Add(0); // 햄버거는 무조건 주문
        int ran1, ran2;
        ran1 = Random.Range(0, 2);
        ran2 = Random.Range(0, 2);
        if(ran1 == 1) orderMenu.Add(1); // 2분의 1 확률로 감자튀김 주문
        if (ran2 == 1) orderMenu.Add(2); // 2분의 1 확률로 음료수 주문

        StartCoroutine(Serve());
    }

    // 서빙 완료를 확인하기 위한 테스트 코드
    IEnumerator Serve()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log(targetTableNum + " 서빙 완료");
        isServe = true;
    }

    // 식사
    void Eat()
    {
        if (isServe) // 서빙이 됐다면
        {
            eatTime += Time.deltaTime; // 식사 시간 타이머
            if(eatTime >= eatCycle) // 식사 시간이 일정 시간에 도달하면
            {
                Debug.Log(targetTableNum + " 퇴장");
                eatTime = 0; 
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Euler(0,180,0);
                isExit = true;
                isServe = false;
            }
        }
    }

    // 입구로 나간 후, 각종 변수들 초기화
    void Final() 
    {
        Debug.Log(targetTableNum + " 퇴장 완료");
        isEntry = !isEntry; isExit = !isExit;
        orderMenu.Clear();
    }
}