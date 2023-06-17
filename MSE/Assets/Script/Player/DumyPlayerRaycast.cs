using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumyPlayerRaycast : MonoBehaviour
{
    private RaycastHit hit;

    void Update()
    {
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Object"))
            {
                Debug.Log("닿는데?");
                //pick up 색상 변경
                Color target = new Color(1.0f, 1.0f, 0.88f, 0.9f);
                hit.collider.GetComponent<MeshRenderer>().materials[1].SetColor("_Color",target);
            }
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
    }
}
