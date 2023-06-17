using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    private Color temp_color;
    private Color cur_color;

    private void Awake()
    {
        temp_color = this.GetComponent<MeshRenderer>().materials[1].GetColor("_Color");
    }

    void Update()
    {
        cur_color = this.GetComponent<MeshRenderer>().materials[1].GetColor("_Color");

        //색이 변경되면 원래대로 전환
        if (cur_color.a != temp_color.a)
        {
            this.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", temp_color);
        }
    }
}
