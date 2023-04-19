using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3[] vector3s;
    // Update is called once per frame
    void Update()
    {
        //플레이어의 x좌표가 카메라 최대범위 x좌표보다 크고, z좌표는 클 때
        if (target.position.x > vector3s[0].x && target.position.z > vector3s[0].z)
        {
            transform.position = new Vector3(vector3s[0].x, target.position.y, vector3s[0].z) + offset;
        }
        //플레이어의 x좌표가 카메라 최대범위 x좌표보다 크고, z좌표는 작을 때
        else if (target.position.x > vector3s[0].x && target.position.z < vector3s[1].z)
        {
            transform.position = new Vector3(vector3s[0].x, target.position.y, vector3s[1].z) + offset;
        }
        //플레이어의 x좌표가 카메라 최대범위 x좌표보다 작고, z좌표는 클 때
        else if (target.position.x < vector3s[1].x && target.position.z > vector3s[0].z)
        {
            transform.position = new Vector3(vector3s[1].x, target.position.y, vector3s[0].z) + offset;
        }
        //플레이어의 x좌표가 카메라 최대범위 x좌표보다 작고, z좌표는 작을 때
        else if (target.position.x < vector3s[1].x && target.position.z < vector3s[1].z)
        {
            transform.position = new Vector3(vector3s[1].x, target.position.y, vector3s[1].z) + offset;
        }
        //플레이어의 x좌표가 카메라 최대범위 x좌표보다 클 때
        else if (target.position.x > vector3s[0].x)
        {
            transform.position = new Vector3(vector3s[0].x, target.position.y, target.position.z) + offset;
        }
        //플레이어의 x좌표가 카메라 최대범위 x좌표보다 작을 때
        else if (target.position.x < vector3s[1].x)
        {
            transform.position = new Vector3(vector3s[1].x, target.position.y, target.position.z) + offset;
        }
        //플레이어의 z좌표가 카메라 최대범위 z좌표보다 클 때
        else if (target.position.z > vector3s[0].z)
        {
            transform.position = new Vector3(target.position.x, target.position.y, vector3s[0].z) + offset;
        }
        //플레이어의 z좌표가 카메라 최대범위 z좌표보다 작을 때
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
