using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3[] vector3s;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾��� x��ǥ�� ī�޶� �ִ���� x��ǥ���� ũ��, z��ǥ�� Ŭ ��
        if (target.position.x > vector3s[0].x && target.position.z > vector3s[0].z)
        {
            transform.position = new Vector3(vector3s[0].x, target.position.y, vector3s[0].z) + offset;
        }
        //�÷��̾��� x��ǥ�� ī�޶� �ִ���� x��ǥ���� ũ��, z��ǥ�� ���� ��
        else if (target.position.x > vector3s[0].x && target.position.z < vector3s[1].z)
        {
            transform.position = new Vector3(vector3s[0].x, target.position.y, vector3s[1].z) + offset;
        }
        //�÷��̾��� x��ǥ�� ī�޶� �ִ���� x��ǥ���� �۰�, z��ǥ�� Ŭ ��
        else if (target.position.x < vector3s[1].x && target.position.z > vector3s[0].z)
        {
            transform.position = new Vector3(vector3s[1].x, target.position.y, vector3s[0].z) + offset;
        }
        //�÷��̾��� x��ǥ�� ī�޶� �ִ���� x��ǥ���� �۰�, z��ǥ�� ���� ��
        else if (target.position.x < vector3s[1].x && target.position.z < vector3s[1].z)
        {
            transform.position = new Vector3(vector3s[1].x, target.position.y, vector3s[1].z) + offset;
        }
        //�÷��̾��� x��ǥ�� ī�޶� �ִ���� x��ǥ���� Ŭ ��
        else if (target.position.x > vector3s[0].x)
        {
            transform.position = new Vector3(vector3s[0].x, target.position.y, target.position.z) + offset;
        }
        //�÷��̾��� x��ǥ�� ī�޶� �ִ���� x��ǥ���� ���� ��
        else if (target.position.x < vector3s[1].x)
        {
            transform.position = new Vector3(vector3s[1].x, target.position.y, target.position.z) + offset;
        }
        //�÷��̾��� z��ǥ�� ī�޶� �ִ���� z��ǥ���� Ŭ ��
        else if (target.position.z > vector3s[0].z)
        {
            transform.position = new Vector3(target.position.x, target.position.y, vector3s[0].z) + offset;
        }
        //�÷��̾��� z��ǥ�� ī�޶� �ִ���� z��ǥ���� ���� ��
        else if (target.position.z < vector3s[1].z)
        {
            transform.position = new Vector3(target.position.x, target.position.y, vector3s[1].z) + offset;
        }
        
        else
        {
            transform.position = target.position + offset;
        }
        
    }
}
