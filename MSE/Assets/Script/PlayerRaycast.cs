using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    RaycastHit hit;
    public Material[] mat = new Material[2];

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position - new Vector3(0f, 0.6f, 0f), transform.forward, out hit, 1.5f))
        {
            if (hit.collider.tag == "Object")
            {
                hit.collider.GetComponent<Renderer>().material = mat[0];
            }
            /*
            else if(hit.collider.tag == "Object"
                && hit.collider.GetComponent<Renderer>().material.name == "Targetting")
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    hit.collider.GetComponent<Renderer>().material = mat[1];
                }
            }
            */
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
        
        //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
    }
}
