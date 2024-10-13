using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPickup : MonoBehaviour
{
    public PumpTrigger pumpTriggerRedSauce;
    public PumpTrigger pumpTriggerCheese;
    Vector3 sourceEulerAngles;
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
    6 = Onion
    7 = Bean
    8 = Red Sauce
    9 = Nacho Cheese
    */
    public GameObject[] itemHeldPrefab;       // Prefab to instantiate when picked up from the source
    public GameObject[] itemDropPrefab;
    //public float shrinkFactor = 0.5f;         // Factor by which the source object will shrink

    private GameObject heldItem;              // Reference to the currently held item
    public GameObject burrito;
    [SerializeField] private bool isHoldingIngredient = false;       // Check if the player is holding an item
    [SerializeField] private bool isHoldingItem = false;
     [SerializeField] private bool keepRotation = false;

    private int heldItemIndex;

    void Update()
    {
        #region Updates
        // Check if the player is holding an ingredient
        if (!isHoldingIngredient)
        {
            if (Input.GetKeyDown(KeyCode.R) && !isHoldingItem)
            {
                WrapItem();
            }
            else
            {
                // Check for pickup action
                CheckForPickup();
            }
        }
        else
        {
            // If holding an item, allow placing it down
            if (Input.GetMouseButtonDown(1)) // Right-click to place ingredient
            {
                PlaceItem();
            }
            if (Input.GetKey(KeyCode.F) && isHoldingItem)
            {
                DropItem();
            }
            if (keepRotation)
            {
                // Get the rotation of the source object as Euler angles
                sourceEulerAngles = holdPosition.transform.eulerAngles;

                // Set the Y-axis to 0 (or any axis you want to fix)
                sourceEulerAngles.x = 0f;

                GetItemInHand().transform.eulerAngles = sourceEulerAngles;
            }
        }

        if (pumpTriggerRedSauce.dispensedBurrito)
        {
            PlaceInTortilla(pumpTriggerRedSauce.foodType);
        }
        if (pumpTriggerRedSauce.dispensedTaco)
        {
            PlaceInShell(pumpTriggerRedSauce.foodType);
        }
        if (pumpTriggerCheese.dispensedBurrito)
        {
            PlaceInTortilla(pumpTriggerCheese.foodType);
        }
        if (pumpTriggerCheese.dispensedTaco)
        {
            PlaceInShell(pumpTriggerCheese.foodType);
        }

        #endregion
    }
    
    GameObject GetItemInHand()
    {
        return holdPosition.GetChild(0).gameObject;
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
                    case "Tortilla":
                        PickTacoUp(sourceObject);
                        break;
                    case "Burrito":
                        PickupObject(sourceObject);
                        break;
                    case "OpenBag":
                        PickupObject(sourceObject);
                        break;
                    case "WrappedTaco":
                        PickupObject(sourceObject);
                        break;
                    case "WrappedBurrito":
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
                    case "OnionSource":
                        heldItemIndex = 6;
                        // Shrink the source object
                        objHP.MoveDown();

                        // Instantiate the item prefab and hold it
                        PickupIngredient(heldItemIndex);
                        break;
                    case "BeanSource":
                        heldItemIndex = 7;
                        // Shrink the source object
                        objHP.MoveDown();

                        // Instantiate the item prefab and hold it
                        PickupIngredient(heldItemIndex);
                        break;
                    case "RedSauceSource":
                        heldItemIndex = 8;
                        pumpTriggerRedSauce.isPumped = true;
                        break;
                    case "NachoCheeseSource":
                        heldItemIndex = 9;
                        pumpTriggerCheese.isPumped = true;
                        break;
                }
            }
        }
        #endregion
    }

    void PickTacoUp(GameObject tacoShell)
    {
        #region Pick Up Taco
        heldItemIndex = -1;
        
        if (tacoShell.tag == "Tortilla")
        {
            tacoShell = tacoShell.transform.parent.gameObject;
        }
        
        // Turn off gravity & enable restraints
        Rigidbody rb = tacoShell.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        // Set the item to be a child of the hold position
        tacoShell.transform.SetParent(holdPosition);

        // Adjust the position to make it look like it's in hand
        tacoShell.transform.position = holdPosition.position;

        // Apply the modified rotation to the target object
        tacoShell.transform.eulerAngles = sourceEulerAngles;

        // Update the holding status
        isHoldingIngredient = true;
        isHoldingItem = true;
        keepRotation = true;
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
        switch (item.gameObject.tag)
        {
            case "SourCreamGun":
                item.transform.rotation = holdPosition.rotation * Quaternion.Euler(-90, 0, 0);
                item.GetComponent<Collider>().isTrigger = true;
                break;
            case "Burrito":
                item.transform.rotation = holdPosition.rotation * Quaternion.Euler(0, 0, -90);
                break;
            case "WrappedBurrito":
                item.transform.rotation = holdPosition.rotation * Quaternion.Euler(0, 0, -90);
                break;
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
        keepRotation = false;
        #endregion
    }

    // Function to place down the item and create a new instance
    void PlaceItem()
    {
        #region Place Item
        RaycastHit hit;
        if (isHoldingItem && heldItemIndex != 5 && heldItemIndex != -1)
        {
            return;
        }
        
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
                case "Tortilla":
                    PlaceInTortilla(sourceObject);
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
                case "OnionSource":
                    if(heldItemIndex == 6)
                    {
                        objHP.MoveUp();
                        DestroyItemInHand();
                    }
                    break;
                case "BeanSource":
                    if(heldItemIndex == 7)
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
        float tacoShellOffsetFromCenter = 0.08f;
        //float ingredientPosition = (ingredientHeight/2) - tacoShellOffsetFromCenter + lastIngredientHeight;
        float ingredientPosition = (ingredientHeight/2) + tacoScript.GetTotalIngredientHeight() - tacoShellOffsetFromCenter;
        //Debug.Log(ingredientPosition);
        ///tacoScript.PrintOutAllIngredients();
        tacoScript.UpdateList();

        //ingredient.transform.localPosition = new Vector3(0, tacoScript.ingredientCount * tacoScript.stackValue, 0);
        ingredient.transform.localPosition = new Vector3(0, ingredientPosition, 0);
        ingredient.transform.rotation = shellPosition.rotation;

        pumpTriggerRedSauce.dispensedTaco = false;
        pumpTriggerCheese.dispensedTaco = false;
        pumpTriggerRedSauce.isPumped = false;
        pumpTriggerCheese.isPumped = false;
        #endregion
    }

    void PlaceInTortilla(GameObject tortilla)
    {
        Debug.Log("TORTILLA");
        #region Place In Tortilla
        BurritoScript burritoScript = tortilla.GetComponent<BurritoScript>();
        Transform tortillaPosition = tortilla.transform;
        GameObject ingredient = Instantiate(itemDropPrefab[heldItemIndex],tortillaPosition.position,tortillaPosition.rotation);
        ingredient.name = itemDropPrefab[heldItemIndex].name;

        ingredient.transform.SetParent(tortillaPosition.parent);
        Vector3 currentScale = ingredient.transform.localScale;

        currentScale.z *= 3f;
        // Will reduce a tall item like lettuce to a reasonable size
        if (heldItemIndex == 0)
        {
            currentScale.y *= .5f;
        }

        ingredient.transform.localScale = currentScale;
        
        float ingredientHeight = ingredient.transform.localScale.y;
        float ingredientPosition = (ingredientHeight/2) + burritoScript.GetTotalIngredientHeight();

        burritoScript.UpdateList();

        ingredient.transform.localPosition = new Vector3(0, ingredientPosition, 0);
        ingredient.transform.rotation = tortillaPosition.rotation;

        pumpTriggerRedSauce.dispensedBurrito = false;
        pumpTriggerCheese.dispensedBurrito = false;
        pumpTriggerRedSauce.isPumped = false;
        pumpTriggerCheese.isPumped = false;
        #endregion
    }

    void WrapItem()
    {
        #region Wrap Item
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange, pickupMask))
        {
            GameObject sourceObject = hit.transform.gameObject;
            if (sourceObject.tag == "Tortilla")
            {
                Vector3 newPos = new Vector3(sourceObject.transform.position.x, 
                    sourceObject.transform.position.y + burrito.transform.localScale.x/2, 
                    sourceObject.transform.position.z);
                Quaternion newRot = Quaternion.Euler(sourceObject.transform.rotation.x,
                sourceObject.transform.rotation.y,
                sourceObject.transform.rotation.z - 90);
                BurritoScript burritoScript = sourceObject.GetComponent<BurritoScript>();
                GameObject newBurrito = Instantiate(burrito, newPos, newRot);
                BurritoContent burritoContent = newBurrito.GetComponent<BurritoContent>();

                burritoContent.SetIngredientsList(burritoScript.ingredientsNames);

                Destroy(sourceObject.transform.parent.gameObject);
            }
        }
        #endregion
    }

    void DestroyItemInHand()
    {
        Destroy(heldItem);
        heldItem = null;
        isHoldingIngredient = false;
    }
}
