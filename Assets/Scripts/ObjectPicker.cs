using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public Transform holdPosition;            // The position where the object will be held (e.g., in front of the player)
    public float pickupRange = 3f;            // Maximum distance for picking up objects
    public LayerMask pickupMask;              // Layer mask to specify which objects can be picked up
    public GameObject replacementPrefab;      // Prefab to instantiate when placing down the object

    private GameObject pickedUpObject;        // Reference to the currently picked-up object
    private bool isHoldingObject = false;     // Check if player is holding an object

    void Update()
    {
        // If not holding an object, check for potential objects to pick up
        if (!isHoldingObject)
        {
            CheckForPickup(); 
        }
        else
        {
            // If holding an object, allow placing it down
            if (Input.GetMouseButtonDown(1)) // Right-click to place down
            {
                PlaceObject();
            }
        }
    }

    // Raycast to check for objects to pick up
    void CheckForPickup()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to pick up
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange, pickupMask))
            {
                GameObject objectHit = hit.transform.gameObject;

                // Pick up the object
                PickupObject(objectHit);
            }
        }
    }

    // Function to pick up the object
    void PickupObject(GameObject obj)
    {
        // Disable object physics
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable gravity and collision response
        }

        // Set object as child of the hold position (e.g., in front of the camera)
        obj.transform.SetParent(holdPosition);

        // Set the position and rotation of the object relative to the hold position
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Store reference to the picked up object
        pickedUpObject = obj;
        isHoldingObject = true;
    }

    // Function to place down the object and replace it with a different instance
    void PlaceObject()
    {
        // Save the current position and rotation for placing the new object
        Vector3 placePosition = pickedUpObject.transform.position;
        Quaternion placeRotation = pickedUpObject.transform.rotation;

        // Destroy or deactivate the original picked-up object
        Destroy(pickedUpObject);

        // Instantiate the new object (replacementPrefab) at the same position
        Instantiate(replacementPrefab, placePosition, placeRotation);

        // Clear the reference to the picked-up object
        pickedUpObject = null;
        isHoldingObject = false;
    }
}