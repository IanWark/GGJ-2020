using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMgr : MonoBehaviour
{
    // These two are just for getting the data to then create the Ingredients at run time
    public List<SymptomChangeList> symptomChangeLists = new List<SymptomChangeList>();
    public List<IngredientView> ingredientViews = new List<IngredientView>();

    // The actual data structure used at run time for the ingredients
    private List<Ingredient> ingredients = new List<Ingredient>();

    void Start()
    {
        for (int i = 0; i < symptomChangeLists.Count; ++i)
        {
            ingredients.Add(new Ingredient(symptomChangeLists[i], ingredientViews[i]));
        }
    }
}
