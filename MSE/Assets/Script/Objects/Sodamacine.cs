using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sodamacine : MonoBehaviour
{
    public Transform[] Spwanpoints;
    public bool Is_interact = false;

    [SerializeField]
    private GameObject food;

    private GameObject tmp;
    void Update()
    {
        if (Is_interact)
        {
            //스폰포인트 1~4까지 차례대로 스폰
            if(Spwanpoints[0].childCount == 0)
            {
                tmp = Instantiate(food, Spwanpoints[0].position, food.transform.rotation);
                tmp.transform.SetParent(Spwanpoints[0]);
                Is_interact = false;
            }
            else if (Spwanpoints[1].childCount == 0)
            {
                tmp = Instantiate(food, Spwanpoints[1].position, food.transform.rotation);
                tmp.transform.SetParent(Spwanpoints[1]);
                Is_interact = false;
            }
            else if (Spwanpoints[2].childCount == 0)
            {
                tmp = Instantiate(food, Spwanpoints[2].position, food.transform.rotation);
                tmp.transform.SetParent(Spwanpoints[2]);
                Is_interact = false;
            }
            else if (Spwanpoints[3].childCount == 0)
            {
                tmp = Instantiate(food, Spwanpoints[3].position, food.transform.rotation);
                tmp.transform.SetParent(Spwanpoints[3]);
                Is_interact = false;
            }
            Is_interact =false;
        }
    }
}
