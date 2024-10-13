using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrapper : MonoBehaviour
{
    public String foodType;
    public List<string> ingredients;
    public GameObject tacoWrap;
    public GameObject burritoWrap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.parent == null && other.gameObject.layer == 6)
        {
            if (other.gameObject.tag == "TacoShell")
            {
                foodType = "TacoShell";
                if (other.transform.childCount > 0)
                {
                    foreach (Transform child in other.transform)
                    {
                        ingredients.Add(child.gameObject.name);
                    }
                }
                GameObject tacoWrapObj = Instantiate(tacoWrap, transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(0, other.transform.rotation.eulerAngles.y, 0));
                tacoWrapObj.GetComponent<WrapperContent>().SetIngredientsList(foodType, ingredients);
                Destroy(other.gameObject);
                Destroy(transform.parent.gameObject);
            }
            if (other.gameObject.tag == "Burrito")
            {
                other.gameObject.tag = "WrappedBurrito"; // So other wrappers cant wrap the same burrito
                BurritoContent burritoScript = other.gameObject.GetComponent<BurritoContent>();
                foodType = "Tortilla";
                ingredients = burritoScript.ingredients;
                Transform burritoPosition = other.gameObject.transform;
                Destroy(other.gameObject);

                GameObject burritoWrapObj = Instantiate(burritoWrap, transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(0, burritoPosition.rotation.eulerAngles.y, -90));
                burritoWrapObj.GetComponent<WrapperContent>().SetIngredientsList(foodType, ingredients);
                Destroy(transform.parent.gameObject);
            }
        }
   
    }
}
