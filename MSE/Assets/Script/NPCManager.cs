using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance;

    public int selectNPC;
    [SerializeField] Transform spawn;

    public float spawnTimer = 0f; // 스폰 주기를 위한 타이머 => 추후 스폰 주기를 디자인해서 수정한다.
    public float cycle = 5f; // NPC 스폰하는 주기
    public int NPCCount; // NPC가 스폰된 수 확인 

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(NPCCount >= transform.childCount) spawnTimer = 0f;
        if(spawnTimer >= cycle)
        {
            spawnTimer = 0f;
            Spawn();
            NPCCount++;
        }
    }

    // 어떤 NPC를 내보낼 지 랜덤으로 고른다.
    void Spawn()
    {
        selectNPC = Random.Range(0, transform.childCount);
        if (!transform.GetChild(selectNPC).gameObject.activeSelf) // 이미 스폰된 NPC는 다시 스폰 안되게끔
        {
            transform.GetChild(selectNPC).position = spawn.position + new Vector3(0,1,0);
            transform.GetChild(selectNPC).gameObject.SetActive(true);
        }
        else
        {
            Spawn();
        }
    }
}