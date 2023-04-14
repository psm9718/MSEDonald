using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField]
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<Renderer>().material.name == "Targetting (Instance)")
        {
            this.GetComponent<Renderer>().material = mat;
        }
    }
}
