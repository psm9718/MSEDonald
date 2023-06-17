using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject[] Pick_Up;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            for (int i = 0; i < Pick_Up.Length; i++)
            {
                // done 상태인 Food_Out중 하나를 고르고 상태 변경, Food_Out에 있는 오브젝트들 제거
                if (Pick_Up[i].gameObject.GetComponent<Food_Out>().menu == Food_Out.Menu.done)
                {
                    Pick_Up[i].gameObject.GetComponent<Food_Out>().NPC_pick = true;
                    Pick_Up[i].gameObject.GetComponent<Food_Out>().menu = Food_Out.Menu.wait;
                    NPCManager.instance.NPCCount--;
                    break;
                }
            }
        }
    }
}
