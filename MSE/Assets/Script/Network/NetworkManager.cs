using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using System;
using System.Threading;
using System.Text;
using UnityEngine.Networking.PlayerConnection;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public WebSocketSharp.WebSocket ws = null;
    [Header("Button")]
    public Button P1;
    public Button P2;
    public Button Ready;
    [Header("Text")]
    public Text main;
    public Text P1ready;
    public Text P2ready;
    public Text state;
    [Header("Panel")]
    public GameObject login_panel;
    public GameObject select_panel;
    public int _Myid = 0;
    public int result;
    //private bool _Isready;

    public SignPayload SendData;
    public SignPayload ReceiveData = null;

    // Start is called before the first frame update
    void Start()
    {       
        Instance = this;
        ws = new WebSocketSharp.WebSocket("ws://43.201.219.112:8080/api/socket");

        ws.OnMessage += OnWebSocketMessage;
        ws.Connect();

        P1ready.gameObject.SetActive(false);
        P2ready.gameObject.SetActive(false);
    }

    public void OnWebSocketMessage(object sender, WebSocketSharp.MessageEventArgs e)
    {
        ReceiveData = JsonUtility.FromJson<SignPayload>(e.Data);
        
    }

    void Update()
    {
        result = SendData._id + ReceiveData._id;
        if (result == 3)
        {
            this.GetComponent<AudioSource>().mute = true;
            SceneManager.LoadScene("IngameScene");
        }

        if (ReceiveData._P1ready)
        {
            P1ready.gameObject.SetActive(true);
            SendData._P1ready = false;
        }else if (ReceiveData._P2ready)
        {
            P2ready.gameObject.SetActive(true);
            SendData._P2ready = false;
        }

    }

    public void P1Button()
    {
        main.text = "P1";

        _Myid = 1;

        TokenManager.instance._playerNum = _Myid;

        P1.interactable = false;

        if (!P2ready.gameObject.activeSelf)
        {
            P2.interactable = true;
        }
        
    }

    public void P2Button()
    {
        main.text = "P2";

        _Myid = 2;

        TokenManager.instance._playerNum = _Myid;

        P2.interactable = false;

        if (!P1ready.gameObject.activeSelf)
        {
            P1.interactable = true;
        }

        
    }

    public void ReadyButton()
    {
        Ready.interactable = false;

        SendData._id = _Myid;

        if (!P1.interactable)
        {
            P1.interactable = false;
            P1ready.gameObject.SetActive(true);
            SendData._P1ready = true;
        }
        else if(!P2.interactable)
        {
            P2.interactable = false;
            P2ready.gameObject.SetActive(true);
            SendData._P2ready = true;
        }
        //_Isready = true;

        string SignPayloadJson = JsonUtility.ToJson(SendData);

        ws.Send(SignPayloadJson);
    }

    public void BackButton()
    {
        select_panel.SetActive(false);
        login_panel.SetActive(true);
        state.text = "";
    }

    public void Exit()
    {
        Application.Quit();
    }

    [System.Serializable]
    public class SignPayload
    {
        public int _id = 0;
        public bool _P1ready;
        public bool _P2ready;
    }

}
