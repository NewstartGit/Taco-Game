using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpTrigger : MonoBehaviour
{
    public bool isPumped = false;
    public bool dispensedBurrito = false;
    public bool dispensedTaco = false;
    public GameObject foodType;

    void Update()
    {
        if (foodType != null)
        {
            if (foodType.tag == "Tortilla" && isPumped)
            {
                Debug.Log(foodType.name);
                dispensedBurrito = true;
            }
            if (foodType.tag == "TacoShell" && isPumped)
            {
                dispensedTaco = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        Debug.Log("TRIGGER");
        if (other.gameObject.layer == 6)
        {
            foodType = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 6)
        {
            foodType = null;
        }
    }
}
