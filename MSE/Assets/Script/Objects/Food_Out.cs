using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Out : MonoBehaviour
{
    public enum Menu {single, cokeset, friedset, fullset, done, wait}
    public Menu menu;
    public static Food_Out Instance;

    [SerializeField]
    private GameObject[] foods;

    public GameObject NPCmanager;
    public GameObject Menu_UI;

    public bool Is_interact = false;
    public bool Is_order = false;
    public bool NPC_pick = false;

    void Awake()
    {
        //초기화
        Instance = this;
        menu = Menu.wait;
        Menu_UI.SetActive(false);
    }

    void Update()
    {
        if (Is_interact)
        {
            //카운터 위에 음식 올려두기
            if (Player.Instance._role == Player.Role.Cook)
            {
                if (!foods[3].gameObject.activeSelf)
                {
                    foods[3].gameObject.SetActive(true);
                }
                switch (Player.Instance.hand_index)
                {
                    case 0:
                        foods[0].gameObject.SetActive(true);
                        break; 
                    case 1:
                        foods[1].gameObject.SetActive(true);
                        break; 
                    case 2:
                        foods[2].gameObject.SetActive(true);
                        break;
                }
            }
            else if(Player.Instance._role == Player.Role.Hall)
            {
                if (!foods[3].gameObject.activeSelf)
                {
                    foods[3].gameObject.SetActive(true);
                }
                switch (DumyPlayer.Instance.hand_index)
                {
                    case 0:
                        foods[0].gameObject.SetActive(true);
                        break;
                    case 1:
                        foods[1].gameObject.SetActive(true);
                        break;
                    case 2:
                        foods[2].gameObject.SetActive(true);
                        break;
                }
            }
            Is_interact = false;
        }

        //NPC가 가져가면 음식 오브젝트들 다 끄기
        if (NPC_pick)
        {
            foods[0].gameObject.SetActive(false);
            foods[1].gameObject.SetActive(false);
            foods[2].gameObject.SetActive(false);
            foods[3].gameObject.SetActive(false);

            NPC_pick = false;
        }

        //메뉴판 띄우기
        if(Is_order)
        {
            //메뉴의 종류에 따라 UI가 다른모양으로 나오도록
            switch(menu)
            {
                case Menu.single:
                    Menu_UI.SetActive(true);
                    Menu_UI.transform.GetChild(1).gameObject.SetActive(false);
                    Menu_UI.transform.GetChild(2).gameObject.SetActive(false);
                    break;

                case Menu.cokeset:
                    Menu_UI.SetActive(true);
                    Menu_UI.transform.GetChild(2).gameObject.SetActive(false);
                    break;

                case Menu.friedset:
                    Menu_UI.SetActive(true);
                    Menu_UI.transform.GetChild(1).gameObject.SetActive(false);
                    break;

                case Menu.fullset:
                    Menu_UI.SetActive(true);
                    break;

            }
            Is_order = false;
        }

        // 메뉴에 따른 점수획득
        switch (menu)
        {
            case Menu.single:
                if (foods[2].gameObject.activeSelf)
                {
                    //점수획득
                    SinglePoint();
                    for(int i = 0; i < NPCmanager.gameObject.transform.childCount; i++)
                    {
                        if (NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state == NPC.State.Wait
                            && NPCmanager.gameObject.transform.GetChild(i).gameObject.activeSelf)
                        {
                            NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state = NPC.State.Order;
                            break;
                        }
                    }
                    menu = Menu.done;
                    NPCManager.instance.Order_count--;
                }
                break;

            case Menu.cokeset:
                if (foods[2].gameObject.activeSelf && foods[0].gameObject.activeSelf)
                {
                    //점수획득
                    CokePoint();
                    for (int i = 0; i < NPCmanager.gameObject.transform.childCount; i++)
                    {
                        if (NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state == NPC.State.Wait
                            && NPCmanager.gameObject.transform.GetChild(i).gameObject.activeSelf)
                        {
                            NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state = NPC.State.Order;
                            break;
                        }
                    }
                    menu = Menu.done;
                    NPCManager.instance.Order_count--;
                }
                break;

            case Menu.friedset:
                if (foods[2].gameObject.activeSelf && foods[1].gameObject.activeSelf)
                {
                    //점수획득
                    FriedPoint();
                    for (int i = 0; i < NPCmanager.gameObject.transform.childCount; i++)
                    {
                        if (NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state == NPC.State.Wait
                            && NPCmanager.gameObject.transform.GetChild(i).gameObject.activeSelf)
                        {
                            NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state = NPC.State.Order;
                            break;
                        }
                    }
                    menu = Menu.done;
                    NPCManager.instance.Order_count--;
                }
                break;

            case Menu.fullset:
                if (foods[2].gameObject.activeSelf && foods[1].gameObject.activeSelf && foods[0].gameObject.activeSelf)
                {
                    //점수획득
                    FullPoint();
                    for (int i = 0; i < NPCmanager.gameObject.transform.childCount; i++)
                    {
                        if (NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state == NPC.State.Wait
                            && NPCmanager.gameObject.transform.GetChild(i).gameObject.activeSelf)
                        {
                            NPCmanager.gameObject.transform.GetChild(i).GetComponent<NPC>().state = NPC.State.Order;
                            break;
                        }
                    }
                    menu = Menu.done;
                    NPCManager.instance.Order_count--;
                }
                break;

            case Menu.done:
                Menu_UI.transform.GetChild(1).gameObject.SetActive(true);
                Menu_UI.transform.GetChild(2).gameObject.SetActive(true);
                Menu_UI.transform.GetChild(3).gameObject.SetActive(true);
                Menu_UI.SetActive(false);
                break;
        }
    }

    void SinglePoint()
    {
        if (Player.Instance._role == Player.Role.Cook)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value += 0.004f;
            }
            else
            {
                Gamemanager.instance.slider.value += 0.002f;
            }
        }
        else if(Player.Instance._role == Player.Role.Hall)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value -= 0.004f;
            }
            else
            {
                Gamemanager.instance.slider.value -= 0.002f;
            }
        }
    }

    void CokePoint()
    {
        if (Player.Instance._role == Player.Role.Cook)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value += 0.008f;
            }
            else
            {
                Gamemanager.instance.slider.value += 0.004f;
            }
        }
        else if (Player.Instance._role == Player.Role.Hall)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value -= 0.008f;
            }
            else
            {
                Gamemanager.instance.slider.value -= 0.004f;
            }
        }
    }

    void FriedPoint()
    {
        if (Player.Instance._role == Player.Role.Cook)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value += 0.012f;
            }
            else
            {
                Gamemanager.instance.slider.value += 0.006f;
            }
        }
        else if (Player.Instance._role == Player.Role.Hall)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value -= 0.012f;
            }
            else
            {
                Gamemanager.instance.slider.value -= 0.006f;
            }
        }
    }

    void FullPoint()
    {
        if (Player.Instance._role == Player.Role.Cook)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value += 0.02f;
            }
            else
            {
                Gamemanager.instance.slider.value += 0.01f;
            }
        }
        else if (Player.Instance._role == Player.Role.Hall)
        {
            if (Gamemanager.instance.Is_fever)
            {
                Gamemanager.instance.slider.value -= 0.02f;
            }
            else
            {
                Gamemanager.instance.slider.value -= 0.01f;
            }
        }
    }
}
