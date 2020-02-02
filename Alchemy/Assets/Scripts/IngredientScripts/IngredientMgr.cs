using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMgr : MonoBehaviour
{
    public static IngredientMgr Instance { get; private set; } = null;

    // These two are just for getting the data to then create the Ingredients at run time
    [SerializeField]
    private List<SymptomChangeList> symptomChangeLists = new List<SymptomChangeList>();
    [SerializeField]
    private List<IngredientView> ingredientViews = new List<IngredientView>();

    // The actual data structure used at run time for the ingredients
    public List<Ingredient> ingredientTypes = new List<Ingredient>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RandomizeData();
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void RandomizeData()
    {
        if (symptomChangeLists.Count < ingredientViews.Count) Debug.Log("Will crash because there are less symptom change lists than there are ingredients");

        // randomize the association between symptom change lists and ingredients
        for (int i = ingredientViews.Count; i > 0; --i)
        {
            int changeListIndex = UnityEngine.Random.Range(0, symptomChangeLists.Count);
            int ingredientIndex = UnityEngine.Random.Range(0, ingredientViews.Count);
            ingredientTypes.Add(new Ingredient(symptomChangeLists[changeListIndex], ingredientViews[ingredientIndex]));

            // WARNING: elements in symptomChangeLists and ingredientViews are consumed during initial setup
            // They become invalid after setup!
            symptomChangeLists.RemoveAt(changeListIndex);
            ingredientViews.RemoveAt(ingredientIndex);
        }

		foreach(Ingredient ingre in ingredientTypes)
		{
			string msg = $"{ingre.View.Name}: ";
			foreach(SymptomChange sc in ingre.SymptomChanges.symptomChanges)
			{
				msg += $"({sc.symptom.ToString()},{sc.change}) ";
			}
			Debug.Log(msg);
		}
    }
}
