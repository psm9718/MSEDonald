using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject food;

    private void OnEnable()
    {
        Instantiate(food, this.transform.position, food.transform.rotation);
    }
}
