using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public SymptomChangeList SymptomChanges { get; } = new SymptomChangeList();
    public IngredientView View { get; } = null;

    public Ingredient(SymptomChangeList symptomChanges, IngredientView view)
    {
        SymptomChanges = symptomChanges;
        View = view;
    }
}

[System.Serializable]
public class IngredientView
{
    [SerializeField]
    private string name = string.Empty;
    public string Name { get { return name; } }

    [SerializeField]
    private Sprite sprite = null;
    public Sprite Sprite { get { return sprite; } }
}