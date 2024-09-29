using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class BagScript : MonoBehaviour
{
    public List<GameObject> foodInBag;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void FreezeObject(Rigidbody rb)
    {
        Debug.Log("Object has stopped. Freezing position and rotation.");
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.parent == null && other.gameObject.layer == 6)
        {
            foodInBag.Add(other.gameObject);
            other.transform.SetParent(transform);
            Rigidbody rb = other.GetComponent<Rigidbody>();

        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        foodInBag.Remove(other.gameObject);
        //other.transform.SetParent(null);
    }
}
