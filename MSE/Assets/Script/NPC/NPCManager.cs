using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance;

    public int selectNPC;
    [SerializeField] Transform spawn;

    public float spawnTimer = 0f; // ���� �ֱ⸦ ���� Ÿ�̸� => ���� ���� �ֱ⸦ �������ؼ� �����Ѵ�.
    public float cycle = 5f; // NPC �����ϴ� �ֱ�
    public int NPCCount; // NPC�� ������ �� Ȯ�� 

    public int Order_count; // �ֹ��� �� �ִ� ����
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

            //�ֹ��� 4�� �̻��̸� �ֹ� ����
            if(Order_count < 4)
            {
                Order();
            }

            NPCCount++;
        }

        //������Ű��� ����� ������
        if (receivedata._npcInfo._spawnSig)
        {
            if (!transform.GetChild(receivedata._npcInfo._num).gameObject.activeSelf)
            {
                transform.GetChild(receivedata._npcInfo._num).position = spawn.position + new Vector3(0, 1, 0);
                transform.GetChild(receivedata._npcInfo._num).gameObject.GetComponent<NPC>().state = NPC.State.Wait;
                transform.GetChild(receivedata._npcInfo._num).gameObject.SetActive(true);

                //�ֹ��� 4�� �̻��̸� �ֹ� ����
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

    // � NPC�� ������ �� �������� ����.
    void Spawn()
    {
        ServerManager.instance._gameData._npcInfo._done = false;
        //NPC��ȣ�� �������� ����
        selectNPC = Random.Range(0, transform.childCount);

        // �̹� �����Ǿ� �ִ� NPC�� �ٽ� ���� �ȵǰԲ�
        if (!transform.GetChild(selectNPC).gameObject.activeSelf) 
        {
            transform.GetChild(selectNPC).position = spawn.position + new Vector3(0,1,0);
            transform.GetChild(selectNPC).gameObject.GetComponent<NPC>().state = NPC.State.Wait;
            transform.GetChild(selectNPC).gameObject.SetActive(true);

            spawnTimer = 0f;

            //���� ������
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
            //wait���� Food_Out�� �ϳ� ����
            if (Pick_Up[i].gameObject.GetComponent<Food_Out>().menu == Food_Out.Menu.wait)
            {
                //�������� �޴� ����
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
        //��ȣ�޾����� ó��
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