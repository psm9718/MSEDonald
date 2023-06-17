using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;

    [System.Serializable]
    public class GameData
    {
        public PlayerInfo _playerInfo;
        public NPCInfo _npcInfo;
        public PickUpInfo _pickUpInfo;
        public DirtyInfo _dirtyInfo;
        public bool _leftcontrol = false;
        public bool _leftcontrolDone = false;
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public float move_x;
        public float move_z;
        public float rotation;
        public bool anim_stay;
        public bool anim_none;
        public bool anim_handle;
    }

    [System.Serializable]
    public class NPCInfo
    {
        public bool _spawnSig;
        public bool _done;
        public int _num;
    }

    [System.Serializable]
    public class DirtyInfo
    {
        public bool _spawnSig;
        public bool _done;
        public int _idx;
    }

    [System.Serializable]
    public class PickUpInfo
    {
        public bool _order;
        public int _menu;
        public int _PickUpNum;
    }

    public GameData _gameData;
    public GameData _receiveData;

    public WebSocketSharp.WebSocket ws = null;

    void Start()
    {
        instance = this;

        ws = new WebSocketSharp.WebSocket("ws://43.201.219.112:8080/api/socket");

        ws.OnMessage += OnWebSocketMessage;

        ws.Connect();

    }

    void Update()
    {
        string send_info = JsonUtility.ToJson(_gameData);

        ws.Send(send_info);
    }

    public void OnWebSocketMessage(object sender, WebSocketSharp.MessageEventArgs e)
    {
        _receiveData = JsonUtility.FromJson<GameData>(e.Data);
    }

    private void OnApplicationQuit()
    {
        _gameData = null;
        ws.Close();
    }

    public void Sending()
    {
        string send_info = JsonUtility.ToJson(_gameData);

        ws.Send(send_info);
    }

    IEnumerator Delay()
    {
        while(true) {
            string send_info = JsonUtility.ToJson(_gameData);

            ws.Send(send_info);

            yield return new WaitForSeconds(0.01f);
        }
        
    }
}
