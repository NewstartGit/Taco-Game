using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperContent : MonoBehaviour
{
    public String foodType;
    public List<String> ingredients;

    public void SetIngredientsList(String foodType, List<String> ingredients)
    {
        this.foodType = foodType;
        this.ingredients = ingredients;
    }
}
