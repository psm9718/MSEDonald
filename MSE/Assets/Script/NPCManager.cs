using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance;

    public int selectNPC;
    [SerializeField] Transform spawn;

    public float spawnTimer = 0f; // ���� �ֱ⸦ ���� Ÿ�̸� => ���� ���� �ֱ⸦ �������ؼ� �����Ѵ�.
    public float cycle = 5f; // NPC �����ϴ� �ֱ�
    public int NPCCount; // NPC�� ������ �� Ȯ�� 

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

    // � NPC�� ������ �� �������� ����.
    void Spawn()
    {
        selectNPC = Random.Range(0, transform.childCount);
        if (!transform.GetChild(selectNPC).gameObject.activeSelf) // �̹� ������ NPC�� �ٽ� ���� �ȵǰԲ�
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