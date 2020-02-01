﻿using System;
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
        // randomize the association between symptom change lists and ingredients
		Random randomness = new Random( DateTime.Now.Millisecond ); // use current time for randomness
		for (int i = ingredientViews.Count; i > 0; --i)
        {
			int changeListIndex = randomness.Next()%symptomChangeLists.Count();
			int ingredientIndex = randomness.Next()%ingredientViews.Count();
			ingredients.Add(new Ingredient(symptomChangeLists[changeListIndex], ingredientViews[ingredientIndex]));

			// WARNING: elements in symptomChangeLists and ingredientViews are consumed during initial setup
			// They become invalid after setup!
			symptomChangeLists.RemoveAt(changeListIndex);
			ingredientViews.RemoveAt(ingredientIndex);
        }
    }
}
