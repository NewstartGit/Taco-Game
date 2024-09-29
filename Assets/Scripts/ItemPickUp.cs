using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPickup : MonoBehaviour
{
    public Transform holdPosition;           // Position where the new object will be held
    public float pickupRange = 3f;           // Maximum distance for picking up items
    public LayerMask pickupMask;              // Layer mask to specify which objects can be picked up
    /*
    0 = Lettuce
    1 = Tomato
    2 = Beef
    3 = Cheese
    4 = Bag
    5 = Sour Cream
    */
    public GameObject[] itemHeldPrefab;       // Prefab to instantiate when picked up from the source
    public GameObject[] itemDropPrefab;
    //public float shrinkFactor = 0.5f;         // Factor by which the source object will shrink

    private GameObject heldItem;              // Reference to the currently held item
    [SerializeField] private bool isHoldingIngredient = false;       // Check if the player is holding an item
    [SerializeField] private bool isHoldingItem = false;
    private int heldItemIndex;

    void Update()
    {
        #region Updates
        // Check if the player is holding an ingredient
        if (!isHoldingIngredient)
        {
            // Check for pickup action
            CheckForPickup();
        }
        else
        {
            //CheckForItemHeld();
            // If holding an item, allow placing it down
            if (Input.GetMouseButtonDown(1)) // Right-click to place ingredient
            {
                PlaceItem();
            }
            if (Input.GetKey(KeyCode.F) && isHoldingItem)
            {
                DropItem();
            }
        }
        #endregion
    }

    void CheckForItemHeld()
    {
        switch(heldItem.tag)
        {
            case "LettuceSource":
                heldItemIndex = 0;
                break;
            case "TomatoSource":
                heldItemIndex = 1;
                break;
        }
    }
 
    // Raycast to check for objects to pick up
    void CheckForPickup()
    {
        #region Handles Pickup
        if (Input.GetMouseButtonDown(0)) // Left-click to pick up
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange, pickupMask))
            {
                GameObject sourceObject = hit.transform.gameObject;
                SourceItemHP objHP = sourceObject.GetComponent<SourceItemHP>();

                // Check if the source object can be picked up
                switch(sourceObject.tag)
                {
                    case "TacoShell":
                        PickTacoUp(sourceObject);
                        break;
                    case "OpenBag":
                        PickupObject(sourceObject);
                        break;
                    case "WrappedTaco":
                        PickupObject(sourceObject);
                        break;
                    case "LettuceSource":
                        heldItemIndex = 0;
                        // Shrink the source object
                        objHP.MoveDown();

                        // Instantiate the item prefab and hold it
                        PickupIngredient(heldItemIndex);
                        break;
                    case "TomatoSource":
                        heldItemIndex = 1;
                        // Shrink the source object
                        objHP.MoveDown();

                        // Instantiate the item prefab and hold it
                        PickupIngredient(heldItemIndex);
                        break;
                    case "BeefSource":
                        heldItemIndex = 2;
                        // Shrink the source object
                        objHP.MoveDown();

                        // Instantiate the item prefab and hold it
                        PickupIngredient(heldItemIndex);
                        break;
                    case "CheeseSource":
                        heldItemIndex = 3;
                        // Shrink the source object
                        objHP.MoveDown();

                        // Instantiate the item prefab and hold it
                        PickupIngredient(heldItemIndex);
                        break;
                    case "FoldedBag":
                        heldItemIndex = 4;

                        PickupIngredient(heldItemIndex);
                        Destroy(sourceObject);
                        break;
                    case "SourCreamGun":
                        heldItemIndex = 5;

                        PickupObject(sourceObject);
                        break;
                }
            }
        }
        #endregion
    }

    void PickTacoUp(GameObject tacoShell)
    {
        #region Pick Up Taco
        
        // Turn off gravity & enable restraints
        Rigidbody rb = tacoShell.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        /*
        // Children restraints

        if (tacoShell.transform.childCount > 0)
        {
            foreach (Transform child in tacoShell.transform)
            {
                Rigidbody childRb = child.GetComponent<Rigidbody>();
                childRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }
        */

        // Set the item to be a child of the hold position
        tacoShell.transform.SetParent(holdPosition);

        // Adjust the position to make it look like it's in hand
        tacoShell.transform.position = holdPosition.position;
        tacoShell.transform.rotation = holdPosition.rotation;

        // Update the holding status
        isHoldingIngredient = true;
        isHoldingItem = true;
        #endregion
    }

    void PickupObject(GameObject item)
    {
        #region Pick Up Object
        
        // Turn off gravity & enable restraints
        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        //rb.isKinematic = true;

        // Set the item to be a child of the hold position
        item.transform.SetParent(holdPosition);

        // Adjust the position to make it look like it's in hand
        item.transform.position = holdPosition.position;
        item.transform.rotation = holdPosition.rotation;

        // Conditions for weird ones
        if (item.gameObject.tag == "SourCreamGun")
        {
            item.transform.rotation = holdPosition.rotation * Quaternion.Euler(-90,0,0);
            item.GetComponent<Collider>().isTrigger = true;
        }

        // Update the holding status
        isHoldingIngredient = true;
        isHoldingItem = true;
        #endregion
    }

    // Function to pick up the item
    void PickupIngredient(int itemIndex)
    {
        #region Pick Up Ingredient
        // Instantiate the item prefab at the hold position
        heldItem = Instantiate(itemHeldPrefab[itemIndex], holdPosition.position, holdPosition.rotation);

        // Set the item to be a child of the hold position
        heldItem.transform.SetParent(holdPosition);

        // Adjust the position to make it look like it's in hand
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        // Update the holding status
        isHoldingIngredient = true;
        if (itemIndex == 4)
        {
            isHoldingItem = true;
            // Turn off gravity & enable restraints
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            if (heldItem.transform.childCount > 0)
            {
                foreach (Transform child in heldItem.transform)
                {
                    Rigidbody childRb = child.GetComponent<Rigidbody>();
                    childRb.useGravity = false;
                    childRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                }
            }
        }
        #endregion
    }

    void DropItem()
    {
        #region Drop Item
        Transform item = holdPosition.GetChild(0);
        Rigidbody rb = item.GetComponent<Rigidbody>();

        item.SetParent(null);
        item.gameObject.GetComponent<Collider>().isTrigger = false;

        //rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        // Children restraints
        /*
        if (item.transform.childCount > 0)
        {
            foreach (Transform child in item.transform)
            {
                Rigidbody childRb = child.GetComponent<Rigidbody>();
                childRb.constraints = RigidbodyConstraints.None;
            }
        }
        */
        isHoldingIngredient = false;
        isHoldingItem = false;
        #endregion
    }

    // Function to place down the item and create a new instance
    void PlaceItem()
    {
        #region Place Item
        RaycastHit hit;
        /*
        if (isHoldingItem)
        {
            Debug.Log("Holding item");
            // If looking at taco shell, apply sour cream, else drop the item
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange))
            {
                GameObject sourceObject = hit.transform.gameObject;
                //SourceItemHP objHP = sourceObject.GetComponent<SourceItemHP>();

                if (sourceObject.tag == "TacoShell") // && hp != 0 //
                {
                    Debug.Log("Taco");
                    PlaceInShell(sourceObject);
                    // Tick down HP //
                    return;
                }
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, .001f))
            {
                Debug.Log("Drop");
                DropItem();
                return;
            }
        }
        */
        //Checking for source
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange, pickupMask))
        {
            GameObject sourceObject = hit.transform.gameObject;
            SourceItemHP objHP = sourceObject.GetComponent<SourceItemHP>();

            // Check if the source object can be picked up
            switch(sourceObject.tag)
            {
                case "TacoShell":
                    PlaceInShell(sourceObject);
                    if (heldItemIndex == 5)
                    {
                        // Tick down HP //
                    }
                    else 
                    {
                        DestroyItemInHand();
                    }
                    break;
                case "LettuceSource":
                    if(heldItemIndex == 0)
                    {
                        objHP.MoveUp();
                        DestroyItemInHand();
                    }
                    break;
                case "TomatoSource":
                    if(heldItemIndex == 1)
                    {
                        objHP.MoveUp();
                        DestroyItemInHand();
                    }
                    break;
                case "BeefSource":
                    if(heldItemIndex == 2)
                    {
                        objHP.MoveUp();
                        DestroyItemInHand();
                    }
                    break;
                case "CheeseSource":
                    if(heldItemIndex == 3)
                    {
                        objHP.MoveUp();
                        DestroyItemInHand();
                    }
                    break;
            }
        }
        #endregion
    }

    void PlaceInShell(GameObject tacoShell)
    {
        #region Place In Shell
        TacoScript tacoScript = tacoShell.GetComponent<TacoScript>();
        Transform shellPosition = tacoShell.transform;
        GameObject ingredient = Instantiate(itemDropPrefab[heldItemIndex],shellPosition.position,shellPosition.rotation);
        ingredient.name = itemDropPrefab[heldItemIndex].name;

        ingredient.transform.SetParent(shellPosition);
        //tacoScript.UpdateList();
        
        float ingredientHeight = ingredient.transform.localScale.y;
        //float lastIngredientHeight = tacoScript.GetLastIngredientHeight();
        float tacoShellOffsetFromCenter = 0.1f;
        //float ingredientPosition = (ingredientHeight/2) - tacoShellOffsetFromCenter + lastIngredientHeight;
        float ingredientPosition = (ingredientHeight/2) + tacoScript.GetTotalIngredientHeight() - tacoShellOffsetFromCenter;
        //Debug.Log(ingredientPosition);
        ///tacoScript.PrintOutAllIngredients();
        tacoScript.UpdateList();

        //ingredient.transform.localPosition = new Vector3(0, tacoScript.ingredientCount * tacoScript.stackValue, 0);
        ingredient.transform.localPosition = new Vector3(0, ingredientPosition, 0);
        ingredient.transform.rotation = shellPosition.rotation;
        #endregion
    }

    void DestroyItemInHand()
    {
        Destroy(heldItem);
        heldItem = null;
        isHoldingIngredient = false;
    }
}
