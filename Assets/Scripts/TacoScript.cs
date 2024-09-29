using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacoScript : MonoBehaviour
{
    [SerializeField] private List<Transform> ingredientsInShell;
    public float stackValue = 0.2f;
    public float totalHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in transform)
        {
            child.localRotation = Quaternion.identity;
        }
    }
    
    public void UpdateList()
    {
        ingredientsInShell.Clear();
        foreach(Transform child in transform)
        {
            ingredientsInShell.Add(child);
        }
    }

    public float GetLastIngredientHeight()
    {
        if (ingredientsInShell.Count == 1)
        {
            return 0;
        }
        else
        {
            return ingredientsInShell[ingredientsInShell.Count - 2].localScale.y;
        }
    }

    public float GetTotalIngredientHeight()
    {
        totalHeight = 0f;
        for (int i = 0; i < ingredientsInShell.Count; i++)
            {
                totalHeight += ingredientsInShell[i].localScale.y;
            }
        return totalHeight;
    }

    public void PrintOutAllIngredients()
    {
        foreach(Transform ingredient in ingredientsInShell)
        {
            Debug.Log(ingredient);
        }
    }
}
