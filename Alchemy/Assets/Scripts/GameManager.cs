using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Sends the new number of patients cured when invoked
    public event Action<int> OnPatientsCuredChanged;

    [SerializeField]
    private PatientManager patientManager = null;

    //[SerializeField]
    //private List<IngredientSlot> ingredientSlots = new List<IngredientSlot>();

    private int patientsCured = 0;
    private int PatientsCured { get { return patientsCured; } set { patientsCured = value; OnPatientsCuredChanged?.Invoke(value); } }

    private void Start()
    {
        patientManager = new PatientManager();
    }

    public void OnBrewPotion()
    {
        List<Ingredient> ingredientList = GetIngredients();
        
        List<SymptomChange> symptomChanges = BrewPotion(ingredientList);

        ApplyChanges(symptomChanges);
    }

    // TODO actually get from UI
    private List<Ingredient> GetIngredients()
    {
        return new List<Ingredient>();
    }

    // TODO actually send to potion resolving class
    private List<SymptomChange> BrewPotion(List<Ingredient> ingredients)
    {
        List<SymptomChange> symptomChanges = new List<SymptomChange>()
        {
            new SymptomChange(eSymptom.sympA, -1)
        };

        return symptomChanges;
    }

    private void ApplyChanges(List<SymptomChange> symptomChanges)
    {
        bool isPatientCured = patientManager.ApplyPotionToPatient(symptomChanges);

        patientsCured += isPatientCured ? 1: 0;
    }
}
