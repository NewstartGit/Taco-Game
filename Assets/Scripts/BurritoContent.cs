using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurritoContent : MonoBehaviour
{
    public List<String> ingredients;

    public void SetIngredientsList(List<String> ingredients)
    {
        this.ingredients = ingredients;
    }
}
