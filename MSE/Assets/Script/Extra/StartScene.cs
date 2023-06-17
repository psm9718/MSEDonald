using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnTextBlink());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("IngameScene");
        }
    }

    public IEnumerator OnTextBlink()
    {
        while (true)
        {
            text.text = "";
            yield return new WaitForSeconds(0.5f);
            text.text = "PRESS TO ENTER";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
