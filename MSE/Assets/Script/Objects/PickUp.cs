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
                // done ������ Food_Out�� �ϳ��� ���� ���� ����, Food_Out�� �ִ� ������Ʈ�� ����
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
