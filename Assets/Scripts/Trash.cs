using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        Destroy(other.gameObject);
    }
}
