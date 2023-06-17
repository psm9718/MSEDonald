using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance;

    public int selectNPC;
    [SerializeField] Transform spawn;

    public float spawnTimer = 0f; // 스폰 주기를 위한 타이머 => 추후 스폰 주기를 디자인해서 수정한다.
    public float cycle = 5f; // NPC 스폰하는 주기
    public int NPCCount; // NPC가 스폰된 수 확인 

    public int Order_count; // 주문이 들어가 있는 수량
    public GameObject[] Pick_Up;
    private int menu_index;
    private ServerManager.GameData receivedata;
    public ServerManager.GameData _sendData;
    void Awake()
    {
        instance = this;

        if(Gamemanager.instance._playerNum == 1)
        {
            spawnTimer += cycle / 2;
        }
    }

    void Update()
    {
        receivedata = ServerManager.instance._receiveData;
        

        if(NPCCount >= transform.childCount)
        {
            if (Gamemanager.instance._playerNum == 1)
            {
                spawnTimer = cycle / 2;
            }
            else
            {
                spawnTimer = 0f;
            }
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
            

        if(spawnTimer >= cycle)
        {
            //spawnTimer = 0f;
            Spawn();

            //주문이 4개 이상이면 주문 안함
            if(Order_count < 4)
            {
                Order();
            }

            NPCCount++;
        }

        //스폰시키라는 통신이 왔을때
        if (receivedata._npcInfo._spawnSig)
        {
            if (!transform.GetChild(receivedata._npcInfo._num).gameObject.activeSelf)
            {
                transform.GetChild(receivedata._npcInfo._num).position = spawn.position + new Vector3(0, 1, 0);
                transform.GetChild(receivedata._npcInfo._num).gameObject.GetComponent<NPC>().state = NPC.State.Wait;
                transform.GetChild(receivedata._npcInfo._num).gameObject.SetActive(true);

                //주문이 4개 이상이면 주문 안함
                if (Order_count < 4)
                {
                    ReceiveOrder();
                }

                ServerManager.instance._gameData._npcInfo._done = true;
                NPCCount++;
            }
        }

        if (receivedata._npcInfo._done)
        {
            ServerManager.instance._gameData._npcInfo._spawnSig = false;
        }
        

    }

    // 어떤 NPC를 내보낼 지 랜덤으로 고른다.
    void Spawn()
    {
        ServerManager.instance._gameData._npcInfo._done = false;
        //NPC번호중 랜덤으로 고르기
        selectNPC = Random.Range(0, transform.childCount);

        // 이미 스폰되어 있는 NPC는 다시 스폰 안되게끔
        if (!transform.GetChild(selectNPC).gameObject.activeSelf) 
        {
            transform.GetChild(selectNPC).position = spawn.position + new Vector3(0,1,0);
            transform.GetChild(selectNPC).gameObject.GetComponent<NPC>().state = NPC.State.Wait;
            transform.GetChild(selectNPC).gameObject.SetActive(true);

            spawnTimer = 0f;

            //정보 보내기
            ServerManager.instance._gameData._npcInfo._spawnSig = true;
            ServerManager.instance._gameData._npcInfo._num = selectNPC;
        }
        else
        {
            Spawn();
        }
    }

    void Order()
    {
        for (int i = 0; i < Pick_Up.Length; i++)
        {
            //wait중인 Food_Out중 하나 선택
            if (Pick_Up[i].gameObject.GetComponent<Food_Out>().menu == Food_Out.Menu.wait)
            {
                //랜덤으로 메뉴 고르기
                menu_index = Random.Range(0, 4);
                switch (menu_index)
                {
                    case 0:
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.single;
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().Is_order = true;

                        SendPickUp(i, menu_index);
                        break;
                    case 1:
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.cokeset;
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().Is_order = true;

                        SendPickUp(i, menu_index);
                        break;
                    case 2:
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.friedset;
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().Is_order = true;

                        SendPickUp(i, menu_index);
                        break;
                    case 3:
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.fullset;
                        Pick_Up[i].gameObject.GetComponent<Food_Out>().Is_order = true;

                        SendPickUp(i, menu_index);
                        break;
                }
                Order_count++;
                return;
            }  
        }
    }

    void ReceiveOrder()
    {
        //신호받았을시 처리
        if (receivedata._pickUpInfo._order)
        {
            switch (receivedata._pickUpInfo._menu)
            {
                case 0:
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.single;
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().Is_order = true;
                    break;
                case 1:
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.cokeset;
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().Is_order = true;
                    break;
                case 2:
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.friedset;
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().Is_order = true;
                    break;
                case 3:
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.fullset;
                    Pick_Up[receivedata._pickUpInfo._PickUpNum].gameObject.GetComponent<Food_Out>().Is_order = true;
                    break;
            }
            receivedata._pickUpInfo._order = false;
            _sendData._pickUpInfo._order = false;
            ServerManager.instance._gameData._pickUpInfo = _sendData._pickUpInfo;
            Order_count++;
        }
    }

    void SendPickUp(int i, int menu_index)
    {
        _sendData._pickUpInfo._order = true;
        _sendData._pickUpInfo._PickUpNum = i;
        _sendData._pickUpInfo._menu = menu_index;
        ServerManager.instance._gameData._pickUpInfo = _sendData._pickUpInfo;
    }
}