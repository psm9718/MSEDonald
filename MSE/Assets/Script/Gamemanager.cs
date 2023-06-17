using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LobbySceneManager;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [Header("Solo")]
    public bool solo;

    public float playtime = 123.0f;
    public float _changetime = 75.0f; //역할 변경 간격
    public float Alpha_time = -0.1f; //카운트다운 페이드아웃 실행용 변수
    public int point;

    private bool _esc = false;
    private string url;

    [Header("UI")]
    [SerializeField]
    private Text time_text;
    [SerializeField]
    private Text Role_text;
    [SerializeField]
    private Text CountDown_text;
    [SerializeField]
    private Text Result_text;
    [SerializeField]
    private Text Point_text;
    [SerializeField]
    private Text Fever_text;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject setting;
    [SerializeField]
    private GameObject End_panel;

    public Slider slider; // 포인트 표현 슬라이더

    public int _playerNum;
    public GameObject[] _players;
    public Transform[] _playerSpawn;
    public bool Is_fever = false;

    private Color _cookColor; //cook역할 색상
    private Color _hallColor; //hall역할 색상
    private string winOrlose;
    private bool once = true;

    Coroutine coroutine;

    private void Awake()
    {
        _playerNum = TokenManager.instance._playerNum;
            
        Time.fixedDeltaTime = 1f / 60f;
        instance = this;
        Is_fever = false;
        Fever_text.gameObject.SetActive(false);
        End_panel.gameObject.SetActive(false);
        setting.SetActive(false);
        slider.value = 0.5f; //슬라이더 절반 위치로 이동
        _cookColor = new Color(0.8078432f, 0.05882353f, 0.0627451f);
        _hallColor = new Color(0.9333334f, 0.8039216f, 0.2784314f);
        coroutine = StartCoroutine(_DecreaseScale(slider));

        if (_playerNum == 1)
        {
            Instantiate(_players[0], _playerSpawn[0].position, _players[0].transform.rotation);
            Instantiate(_players[3], _playerSpawn[1].position, _players[3].transform.rotation);
            ChageColor(Player.Instance._role);
        }
        else if (_playerNum == 2)
        {
            Instantiate(_players[1], _playerSpawn[1].position, _players[1].transform.rotation);
            Instantiate(_players[2], _playerSpawn[0].position, _players[2].transform.rotation);
            ChageColor(Player.Instance._role);
        }
        Time.timeScale = 1;
    }

    void Update()
    {
        _changetime -= Time.deltaTime;
        playtime -= Time.deltaTime;

        ChangeRole(_changetime);

        if(_changetime <= 5f)
        {
            _FadeOut(_changetime);
        }

        if(playtime < 60)
        {
            if (once)
            {
                Fever_text.gameObject.SetActive(true);
                this.GetComponent<AudioSource>().pitch = 1.1f;
                StartCoroutine(OnTextBlink());
                NPCManager.instance.cycle = NPCManager.instance.cycle / 2;
                Is_fever = true;
            }
            time_text.text = "Time: " + string.Format("{0:F2}", playtime);
            once = false;
        }
        else
        {
            time_text.text = $"Time: {(int)(playtime/60)}:" + ((int)playtime - (60 * (int)(playtime / 60))).ToString("D2");
        }

        //게임 종료
        if(playtime < 0)
        {
            this.GetComponent<AudioSource>().mute = true;
            Fever_text.gameObject.SetActive(false);
            time_text.gameObject.SetActive(false);
            Role_text.gameObject.SetActive(false);
            menu.SetActive(false);
            slider.gameObject.SetActive(false);

            End_panel.gameObject.SetActive(true);

            if(slider.value < 0.5)
            {
                winOrlose = "lose";
                Result_text.text = "You Lose!!";
                Point_text.text = $"Point: {slider.value * 100}";
            }
            else if(slider.value >= 0.5) 
            {
                winOrlose = "win";
                Result_text.text = "You Win!!";
                Point_text.text = $"Point: {slider.value * 100}";
            }
            Time.timeScale = 0;
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(_esc)
            {
                if (Is_fever)
                {
                    Fever_text.gameObject.SetActive(true);
                }
                
                time_text.gameObject.SetActive(true);
                Role_text.gameObject.SetActive(true);
                menu.SetActive(true);
                slider.gameObject.SetActive(true);

                setting.SetActive(false);
                _esc = false;
            }
            else
            {
                if (Is_fever)
                {
                    Fever_text.gameObject.SetActive(false);
                }
                time_text.gameObject.SetActive(false);
                Role_text.gameObject.SetActive(false);
                menu.SetActive(false);
                slider.gameObject.SetActive(false);

                setting.SetActive(true);
                _esc = true;
            }
            
        }

        Role_text.text = $"{Player.Instance._role}";
    }

    //게임 재시작 버튼
    public void Resume()
    {
        setting.SetActive(false);

        if (Is_fever)
        {
            Fever_text.gameObject.SetActive(true);
        }
        time_text.gameObject.SetActive(true);
        Role_text.gameObject.SetActive(true);
        menu.SetActive(true);
        slider.gameObject.SetActive(true);
    }

    //게임 종료버튼
    public void Exit()
    {
        PayloadScore scoreForm = new PayloadScore
        {
            score = (double)slider.value * 100,
            winOrLose = winOrlose,
        };
        _SaveScore(scoreForm);
        Application.Quit();
    }

    public void Restart()
    {
        PayloadScore scoreForm = new PayloadScore
        {
            score = (double)slider.value * 100,
            winOrLose = winOrlose,
        };
        _SaveScore(scoreForm);
        SceneManager.LoadScene("LobbyScene");
    }

    //point_bar 증가
    IEnumerator _IncreaseScale(Slider s)
    {
        while(true)
        {
            if (Is_fever)
            {
                s.value += 0.0036f;
            }
            else
            {
                s.value += 0.0018f;
            }
            
            yield return new WaitForSeconds(1);
        }
        
    }

    //point_bar 감소
    IEnumerator _DecreaseScale(Slider s)
    {
        while (true)
        {
            if (Is_fever)
            {
                s.value -= 0.0036f;
            }
            else
            {
                s.value -= 0.0018f;
            }
            yield return new WaitForSeconds(1);
        }

    }

    //point_bar 색상 반전
    void ChageColor(Player.Role role)
    {
        if (role == Player.Role.Cook)
        {
            //background 색깔
            slider.transform.GetChild(0).gameObject.GetComponent<Image>().color = _hallColor;
            //fill 색깔
            slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = _cookColor;

            StopCoroutine(coroutine);
            coroutine = StartCoroutine(_DecreaseScale(slider));

        }
        else if (role == Player.Role.Hall)
        {
            //background 색깔
            slider.transform.GetChild(0).gameObject.GetComponent<Image>().color = _cookColor;
            //fill 색깔
            slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = _hallColor;

            StopCoroutine(coroutine);
            coroutine = StartCoroutine(_IncreaseScale(slider));

            
        }
    }

    // 역할 변경
    void ChangeRole(float time)
    {
        if(time < 0)
        {
            CountDown_text.gameObject.SetActive(false);
            _changetime = 75.0f;
            Player.Instance.Changed();
            ChageColor(Player.Instance._role);
            if (DirtySpawner.Instance._SpawnCount != 0)
            {
                DirtySpawner.Instance.Change = true;
            }
        }
    }

    // 카운트 다운
    void _FadeOut(float time)
    {
        
        if(time < 5f && time >= 4f)
        {
            CountDown_text.gameObject.SetActive(true);
            if (Alpha_time < 0f)
            {
                Alpha_time = 1f;
            }
            else
            {
                CountDown_text.text = "5";
                Alpha_time -= Time.deltaTime;
                CountDown_text.color = new Color(1, 1, 1, Alpha_time);   
            }
            
        }
        else if(time < 4f && time >= 3f)
        {
            if (Alpha_time < 0f)
            {
                Alpha_time = 1f;
            }
            else
            {
                CountDown_text.text = "4";
                Alpha_time -= Time.deltaTime;
                CountDown_text.color = new Color(1, 1, 1, Alpha_time);
            }
        }
        else if (time < 3f && time >= 2f)
        {
            if (Alpha_time < 0f)
            {
                Alpha_time = 1f;
            }
            else
            {
                CountDown_text.text = "3";
                Alpha_time -= Time.deltaTime;
                CountDown_text.color = new Color(1, 1, 1, Alpha_time);
            }
        }
        else if (time < 2f && time >= 1f)
        {
            if (Alpha_time < 0f)
            {
                Alpha_time = 1f;
            }
            else
            {
                CountDown_text.text = "2";
                Alpha_time -= Time.deltaTime;
                CountDown_text.color = new Color(1, 1, 1, Alpha_time);
            }
        }
        else if (time < 1f)
        {
            if (Alpha_time < 0f)
            {
                Alpha_time = 1f;
            }
            else
            {
                CountDown_text.text = "1";
                Alpha_time -= Time.deltaTime;
                CountDown_text.color = new Color(1, 1, 1, Alpha_time);
            }
        }
        else
        {
            Alpha_time = -0.1f;
            return;
        }
    }


    public void _SaveScore(PayloadScore scoreForm)
    {
        url = "http://43.201.219.112:8080/api/scores";
        string json = JsonUtility.ToJson(scoreForm);
        StartCoroutine(SaveScore(url, json));
    }

    // 게임 오버 시 점수를 서버에 보내는 코드 json 처리
    IEnumerator SaveScore(string url, string json)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            Debug.Log("------POST Score Request Start------");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", TokenManager.instance.token);
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                Debug.Log("Request Complete");
            }
            else
            {
                Debug.Log(www.error);
            }
            www.Dispose();
        }
    }

    public IEnumerator OnTextBlink()
    {
        while (true)
        {
            Fever_text.text = "";
            yield return new WaitForSeconds(0.3f);
            Fever_text.text = "Fever!!";
            yield return new WaitForSeconds(0.3f);
        }
    }
}
