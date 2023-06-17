using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    RaycastHit hit;

    void Update()
    {
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f))
        {
            if (hit.collider.tag == "Object")
            {
                //pick up 색상 변경
                Color target = new(1.0f, 1.0f, 0.9f, 0.8f);
                hit.collider.GetComponent<MeshRenderer>().materials[1].SetColor("_Color",target);
            }
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
    }
}
