using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurritoScript : MonoBehaviour
{
    [SerializeField] private List<Transform> ingredientsInTortilla;
    public List<String> ingredientsNames;
    public float stackValue = 0.2f;
    public float totalHeight;

    public bool isWrapped;

    public void SetIngredientsList(List<Transform> ingredients)
    {
        ingredientsInTortilla = ingredients;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWrapped)
        {
            foreach(Transform child in transform.parent)
            {
                child.localRotation = Quaternion.identity;
            }
        }
    }
    
    public void UpdateList()
    {
        ingredientsInTortilla.Clear();
        ingredientsNames.Clear();
        for (int i = 1; i < transform.parent.childCount; i++)
        {
            ingredientsInTortilla.Add(transform.parent.GetChild(i));
            ingredientsNames.Add(transform.parent.GetChild(i).gameObject.name);
        }
        PrintList(ingredientsInTortilla);
    }

    public float GetLastIngredientHeight()
    {
        if (ingredientsInTortilla.Count == 1)
        {
            return 0;
        }
        else
        {
            return ingredientsInTortilla[ingredientsInTortilla.Count - 2].localScale.y;
        }
    }

    public float GetTotalIngredientHeight()
    {
        totalHeight = 0f;
        for (int i = 0; i < ingredientsInTortilla.Count; i++)
        {
            totalHeight += ingredientsInTortilla[i].localScale.y;
        }
        return totalHeight;
    }

    public void PrintOutAllIngredients()
    {
        foreach(Transform ingredient in ingredientsInTortilla)
        {
            Debug.Log(ingredient);
        }
    }

    public void PrintList(List<String> list)
    {
        for (int i = 1; i < transform.parent.childCount; i++)
        {
            Debug.Log(list[i]);
        }
    }

    public void PrintList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }
}
