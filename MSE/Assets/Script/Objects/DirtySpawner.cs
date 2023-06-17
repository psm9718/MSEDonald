using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DirtySpawner : MonoBehaviour
{
    public static DirtySpawner Instance;

    public float _SpawnTime;
    public int _SpawnCount;
    public int spawn_index;
    public bool Change = false;
    private ServerManager.GameData receivedata;

    [SerializeField]
    private Transform[] _SpawnPoints;
    [SerializeField]
    private GameObject _DirtyPrefab;

    GameObject tmp;
    private bool once = true;

    private void Start()
    {
        Instance = this;
        _SpawnTime = NPCManager.instance.cycle;
        _SpawnCount = 0;

        if (Gamemanager.instance._playerNum == 1)
        {
            _SpawnTime = _SpawnTime / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        receivedata = ServerManager.instance._receiveData;
        if (_SpawnCount < 5)
        {
            //�� �����Ӹ��� ��Ÿ�� ����
            _SpawnTime -= Time.deltaTime;
        }
        else
        {
            //5���� �� �����Ǿ��ִ� ��쿡��
            //����Ʈ ��Ÿ���� NPC���� ��Ÿ�Ӱ� ����ȭ ��Ŵ
            _SpawnTime = NPCManager.instance.cycle;

            if (Gamemanager.instance._playerNum == 1)
            {
                _SpawnTime = _SpawnTime / 2;
            }
            
        }
        
        if(Change) 
        {
            for(int i = 0; i < _SpawnPoints.Length; i++)
            {
                if (_SpawnPoints[i].childCount != 0)
                {
                    _SpawnPoints[i].GetChild(0).gameObject.GetComponent<Dirty>().Change = true;
                }
            }
            Change = false;
        }

        //��Ÿ�� �� ���� ����
        if( _SpawnTime < 0 )
        {
            DirtySpawn();
        }

        if (receivedata._dirtyInfo._spawnSig)
        {
            Debug.Log("hi");
            ReceiveSpawn();
            receivedata._dirtyInfo._spawnSig = false;
            ServerManager.instance._gameData._dirtyInfo._done = true;
        }

        if (receivedata._dirtyInfo._done)
        {
            ServerManager.instance._gameData._dirtyInfo._spawnSig = false;
        }
    }

    void DirtySpawn()
    {
        ServerManager.instance._gameData._dirtyInfo._done = false;
        int idx = Random.Range(0, _SpawnPoints.Length);

        //�ڽ� ������Ʈ�� ������ ��Ƽ ����
        if (_SpawnPoints[idx].childCount == 0)
        {
            tmp = Instantiate(_DirtyPrefab, _SpawnPoints[idx].position, _DirtyPrefab.transform.rotation);
            tmp.transform.SetParent(_SpawnPoints[idx]);
            tmp.SetActive(true);
            _SpawnCount++;
            _SpawnTime = 20f;
            if(Gamemanager.instance.playtime < 60f)
            {
                _SpawnTime = _SpawnTime / 2;
                if(once)
                {
                    if (Gamemanager.instance._playerNum == 1)
                    {
                        _SpawnTime = _SpawnTime / 2;
                    }
                    once = false;
                }
                
            }

            ServerManager.instance._gameData._dirtyInfo._idx = idx;
            ServerManager.instance._gameData._dirtyInfo._spawnSig = true;
        }
        else
        {
            DirtySpawn();
        }
    }

    void ReceiveSpawn()
    {
        if (_SpawnPoints[receivedata._dirtyInfo._idx].childCount == 0)
        {
            tmp = Instantiate(_DirtyPrefab, _SpawnPoints[receivedata._dirtyInfo._idx].position, _DirtyPrefab.transform.rotation);
            tmp.transform.SetParent(_SpawnPoints[receivedata._dirtyInfo._idx]);
            tmp.SetActive(true);
            _SpawnCount++;
        }
    }
}
