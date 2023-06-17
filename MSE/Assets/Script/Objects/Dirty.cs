using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dirty : MonoBehaviour
{
    public static Dirty Instance;
    public GameObject _UICanvas;
    public Slider _sliderPrefab;
    public bool Is_interact = false;
    public bool Change = false;
    private int _count = 0;
    Slider slider;
    Camera _camera;
    Coroutine coroutine;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        _UICanvas = GameObject.Find("UICanvas"); // UIĵ���� ������Ʈ�� ã�ƿ�
        _camera = Camera.main;
        slider = Instantiate(_sliderPrefab, this.transform.position, Quaternion.identity);
        slider.transform.SetParent(_UICanvas.transform);
        slider.transform.SetAsFirstSibling();
        if (Player.Instance._role == Player.Role.Cook)
        {
            coroutine = StartCoroutine(_IncreaseScale(Gamemanager.instance.slider));
        }
        else if (Player.Instance._role == Player.Role.Hall)
        {
            coroutine = StartCoroutine(_DecreaseScale(Gamemanager.instance.slider));
        }
    }

    void Update()
    {
        //ȭ�鿡 �����̴� ����
        slider.transform.position = _camera.WorldToScreenPoint(this.transform.position 
            + new Vector3(0f, 1.5f, 0f));

        Interact();

        if(this.Change)
        {
            StopCoroutine(coroutine);
            if (Player.Instance._role == Player.Role.Cook)
            {
                coroutine = StartCoroutine(_IncreaseScale(Gamemanager.instance.slider));
                
            }
            else if (Player.Instance._role == Player.Role.Hall)
            {
                coroutine = StartCoroutine(_DecreaseScale(Gamemanager.instance.slider));
            }
            this.Change = false;
        }

        //�������� 0�� �Ǿ��� ��
        if(slider.value == 0)
        {
            StopCoroutine(coroutine);
            DirtySpawner.Instance._SpawnCount--;

            if(Player.Instance._role == Player.Role.Hall)
            {
                Player.Instance.Is_interact = true;

                if (Gamemanager.instance.Is_fever)
                {
                    Gamemanager.instance.slider.value += 0.01f;
                }
                else
                {
                    Gamemanager.instance.slider.value += 0.005f;
                }
                
            }
            else if (Player.Instance._role == Player.Role.Cook)
            {
                DumyPlayer.Instance._interact = true;

                if (Gamemanager.instance.Is_fever)
                {
                    Gamemanager.instance.slider.value -= 0.01f;
                }
                else
                {
                    Gamemanager.instance.slider.value -= 0.005f;
                }
            }
            
            Destroy(slider.gameObject);
            Destroy(this.gameObject);
        }
    }

    // Interact �޾��� �� �۵�
    void Interact()
    {
        if(Is_interact)
        {
            _count++;
            slider.value = (float)(1 - (0.2 * _count));
            Is_interact = false;
        }
    }

    //point_bar ����
    IEnumerator _IncreaseScale(Slider s)
    {
        while (true)
        {
            if (Gamemanager.instance.Is_fever)
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

    //point_bar ����
    IEnumerator _DecreaseScale(Slider s)
    {
        while (true)
        {
            if (Gamemanager.instance.Is_fever)
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
}
