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

    NavMeshAgent agent; // 네비게이션
    Renderer color;
    bool isCrushed = false; // 부딪힌 적이 있는지 없는지

    [Header(("Transforms"))]
    public Transform entrance; // 가게 입구 및 출구 위치
    public Transform pickUpTable;
    public Transform Tables;
    public List<Transform> targets; // 테이블들의 Transform을 담아두는 array
    public Transform returnPoint; // 반납대

    Vector3 chairPosition; // NPC의 자식 오브젝트가 테이블에 앉아있는 장소를 계산해주는 Vector3
    int chair; // 왼쪽에 앉을지 오른쪽에 앉을지
    
    [Header("NPC Info")]
    public int targetTableNum; // 타겟 테이블 번호
    public float sitPositionX = 1.6f; // 플레이어가 앉을 때 테이블에서 떨어진 거리 
    public float sitPositionZ = 1.7f; // 플레이어가 앉을 때 테이블에서 떨어진 거리
    public float eatTime; // 음식 먹는 시간
    public float eatCycle = 5f; // 한 번 음식 먹는데 걸리는 시간

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
        agent = GetComponent<NavMeshAgent>(); // 네비게이션 초기화
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        // 움직임 
        Moving();
    }

    // NPC 움직임
    void Moving()
    {
        // 음식이 다 만들어진다면 픽업대로 이동 -> 음식이 다 만들어지면 state를 Order로 바꿔서 식당으로 들어오게끔 한다
        if (gameObject.activeSelf && (state == State.Order))
        {
            Walk();
            StartCoroutine(GotoPickUp());
            //state = State.Entry;
        }

        // 픽업대에서 테이블로 이동
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.2f && state == State.Entry){ 
            // 픽업대에 도착했다면
            StartCoroutine(PickUp());
        }

        // 테이블로 도착 후
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.2f && state == State.PickUp) // 도착 판정
        {
            Stay();
            StartCoroutine(Sit());
        }
        
        // NPC 상태가 Eat일 때
        if (state == State.Eat)
        {
            Eat();
        }

        if(state == State.Return)
        {
            Walk();
            GoToReturnTable();
        }

        // 퇴장
        if (state == State.Out)
        {
            agent.SetDestination(entrance.position);
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.9f && state == State.Out)
            {
                transform.position = new Vector3(26, 1, 22);
                gameObject.SetActive(false);
                Debug.Log(targetTableNum + " 퇴장 완료");
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

    // 앉기
    IEnumerator Sit()
    {
        Debug.Log(targetTableNum + " 테이블 도착");
        state = State.Sit;
        agent.enabled = false; // 네비게이션 비활성화
        transform.rotation = Quaternion.Euler(0, 90, 0); // npc가 테이블 방향을 바라보게
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
        transform.position = targets[targetTableNum].position; // npc 위치를 테이블 타겟 위치로 재조정
        if (targetTableNum == 0 || targetTableNum == 3 || targetTableNum == 4 || targetTableNum == 5) { sitPositionX = 2f; }
        transform.GetChild(0).localPosition += chairPosition; // 외형만 이동
        transform.rotation = Quaternion.Euler(0, 0, 0); // 회전값 초기화
        
        if(targetTableNum == 2 || targetTableNum == 11 || targetTableNum == 12)
        {
            transform.GetChild(0).rotation = (chair == 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 270, 0); // 앉은 곳에 따라 외형 회전값 조정
        }
        else
        {
            transform.GetChild(0).rotation = (chair == 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0); // 앉은 곳에 따라 외형 회전값 조정
        }
        state = State.Eat;
    }


    // 픽업
    IEnumerator PickUp()
    {
        yield return new WaitForSeconds(1f);
        agent.SetDestination(targets[targetTableNum].position); // 테이블로 이동
        state = State.PickUp;
    }

    // 식사
    void Eat()
    {
         eatTime += Time.deltaTime; // 식사 시간 타이머
         if (eatTime >= eatCycle) // 식사 시간이 일정 시간에 도달하면
         {
             Debug.Log(targetTableNum + " 퇴장");
             eatTime = 0;
             transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
             transform.rotation = Quaternion.Euler(0, 180, 0);
             state = State.Return;
         }
    }

    void GoToReturnTable()
    {
        transform.GetChild(0).localPosition = new Vector3(0f, -1f, 0f); // 외형 위치 초기화
        agent.enabled = true; // 네비게이션 활성화
        gameObject.GetComponent<Collider>().enabled = true; // 콜라이더 활성화
        agent.SetDestination(returnPoint.transform.position);
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.9f && state == State.Return)
        {
            Debug.Log(targetTableNum + "반납 완료");
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
        // NPC가 Player나 NPC와 부딪히면 넘어진다.
        if (collision.collider.gameObject.CompareTag("Player") || collision.collider.gameObject.CompareTag("Npc"))
        {
            StartCoroutine(Fall());
        }
    }

    // 넘어지는 효과는 깜빡임으로
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

    // 출발 신호
    public void GoToPickUp()
    {
        state = State.Order;
    }
}