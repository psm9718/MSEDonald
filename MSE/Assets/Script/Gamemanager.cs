using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    [SerializeField]
    private Text time_text;
    [SerializeField]
    private Text point_text;
    public float playtime = 123.0f;
    public int point;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        playtime -= Time.deltaTime;
        if(playtime < 60)
        {
            time_text.text = "Time: " + string.Format("{0:F2}", playtime);
            
        }
        else
        {
            time_text.text = $"Time: {(int)(playtime/60)}:" + ((int)playtime - (60 * (int)(playtime / 60))).ToString("D2");
        }

        point_text.text = "point: " + point;
    }
}
