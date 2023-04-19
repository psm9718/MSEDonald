using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingRaycast : MonoBehaviour
{
    RaycastHit hit;
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position - new Vector3(0f, 0.2f, 0f), transform.forward, out hit, 2f))
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (hit.collider.tag == "Object")
                {
                    hit.collider.GetComponent<Soda>().ontarget = true;
                    Player.Instance.Is_interact = true;
                    Player.Instance.food_index = 0;
                }
                else if (hit.collider.tag == "SodaMacine")
                {
                    hit.collider.GetComponent<Sodamacine>().Is_interact = true;
                    
                }
                else if (hit.collider.tag == "Food_Out")
                {
                    if (Player.Instance.On_handle)
                    {
                        Player.Instance.Is_interact = true;
                        hit.collider.GetComponent<Food_Out>().Is_interact = true;
                    }
                    
                }
            }
            
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
        }
    }
}
