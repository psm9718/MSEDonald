using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
    public static LobbySceneManager instance;
    [Header("UI")]
    [SerializeField] private string url;
    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private InputField inputUsername;
    [SerializeField] private InputField inputPassword;
    [SerializeField] private Text stateTxt;
    [SerializeField] private Text[] scoreTxts;
    [Header("Server")]
    [SerializeField] private string token;
    [SerializeField] private PayloadScoreResult[] scoreBoard;
    public double testScore = 1234;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        logInPanel.SetActive(true);
        selectPanel.SetActive(false);
        scorePanel.SetActive(false);
    }

    // 계정 등록
    public void BtnSignUp()
    {
        url = "http://43.201.219.112:8080/api/users";
        Payload signUpForm = new Payload
        {
            username = inputUsername.text,
            password = inputPassword.text,
        };
        string json = JsonUtility.ToJson(signUpForm);
        StartCoroutine(SendSignUp(url, json));
    }

    // 계정 등록 json 등록
    IEnumerator SendSignUp(string url, string json)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.error == null)
            {
                Debug.Log("Sign Up Success");
                stateTxt.color = Color.black;
                stateTxt.text = "Sign Up Success";
                www.Dispose();
            }
            else
            {
                stateTxt.color = Color.red;
                stateTxt.text = "Fail Sign Up";
                Debug.Log("error");
                www.Dispose();
            }
            www.Dispose();
        }
    }

    // 로그인
    public void BtnLogIn()
    {
        url = "http://43.201.219.112:8080/api/users/login";
        Payload logInForm = new Payload
        {
            username = inputUsername.text,
            password = inputPassword.text,
        };
        string json = JsonUtility.ToJson(logInForm);
        StartCoroutine(SendLogIn(url, json));
    }

    // 로그인 json 코드
    IEnumerator SendLogIn(string url, string json)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                // token 저장
                string responseTxt = www.downloadHandler.text;
                string[] jsonResponese = responseTxt.Split('"');
                token = jsonResponese[3];
                TokenManager.instance.token = this.token;

                // 로그인 성공 표시
                stateTxt.color = Color.black;
                stateTxt.text = "Log In !";
                logInPanel.SetActive(false);
                selectPanel.SetActive(true);
            }
            else
            {
                stateTxt.color = Color.red;
                stateTxt.text = "Please check your username or password";
            }
            www.Dispose();
        }
    }

    // 스코어 보드 받아오는 코드
    public void BtnScore()
    {
        url = "http://43.201.219.112:8080/api/scores";
        StartCoroutine(SendScore(url));
        selectPanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    // 스코어 보드 받아오는 코드 json 처리
    IEnumerator SendScore(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            Debug.Log("------Get ScoreBoard Request Start------");
            www.SetRequestHeader("Authorization", TokenManager.instance.token);
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                Debug.Log("Request Complete");
                scoreBoard = JsonHelper.FromJson<PayloadScoreResult>(www.downloadHandler.text);
                for(int i = 0; i < scoreBoard.Length; i++)
                {
                    if(scoreBoard[i].winOrLose == "win"){
                        scoreTxts[i].text = "NO." + (i + 1) + ": Win. Your Points : " + scoreBoard[i].score;
                        scoreTxts[i].color = Color.blue;
                    }
                    else if(scoreBoard[i].winOrLose == "lose")
                    {
                        scoreTxts[i].text = "NO." + (i + 1) + ": Lose. Your Points : " + scoreBoard[i].score;
                        scoreTxts[i].color = Color.red;
                    }
                }
            }
            else
            {
                Debug.Log(www.error);
            }
            www.Dispose();
        }
    }

    // 스코어 보드 Panel Close
    public void BtnCloseScoreBoard()
    {
        scorePanel.SetActive(false);
        selectPanel.SetActive(true);
    }

    // 게임 오버 시 점수를 서버에 보내는 코드
    /*
    ---------사용법---------
    LobbySceneManager.instance.BtnSaveScore(scoreForm)

    scoreForm ->
    PayloadScore scoreForm = new PayloadScore{
            score = double,
            winOrLose = string,
        };

    winOrLose -> "win" or "lose" : 이 문자열 그대로 할 것.
    */
    public void BtnSaveScore(PayloadScore scoreForm) 
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


    // 테스트 용 점수 저장 코드
    public void BtnTestSaveScore()
    {
        url = "http://43.201.219.112:8080/api/scores";
        PayloadScore scoreForm = new PayloadScore
        {
            score = testScore,
            winOrLose = "lose",
        };
        string json = JsonUtility.ToJson(scoreForm);
        StartCoroutine(TestSaveScore(url, json));
    }

    IEnumerator TestSaveScore(string url, string json)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            Debug.Log("------POST Test Send Score Request Start------");
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

    IEnumerator LoadInGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("IngameScene");
    }

    public void testHeader()
    {
        url = "http://43.201.219.112:8080/api/scores";
        StartCoroutine(getTestHeader(url));
    }

    IEnumerator getTestHeader(string url)
    {
        using(UnityWebRequest www = UnityWebRequest.Get(url))
        {
            Debug.Log("------Test Auth------");
            www.SetRequestHeader("Authorization", TokenManager.instance.token);
            yield return www.SendWebRequest();
            if(www.error == null)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    [System.Serializable]
    private class Payload
    {
        public string username;
        public string password;
    }

    public class PayloadScore
    {
        public double score;
        public string winOrLose;
    }

    [System.Serializable]
    private class PayloadScoreResult
    {
        public double score;
        public string username;
        public string winOrLose;
    }

    [System.Serializable]
    private class PayloadToken
    {
        public string token;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}");
            return wrapper.Items;
        }
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    };
}
