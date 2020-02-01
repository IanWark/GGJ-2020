using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Sends the new number of patients cured when invoked
    public event Action<int> OnPatientsCuredChanged;
    // Sends the new number of patients left when invoked
    public event Action<int> OnPatientsToGoChanged;
    // Sends the number of patients cured at the end of the game
    public event Action<int> OnGameEnd;
    // Sends the list of potion experiment results
    public event Action<List<ExperimentResult>> ExperimentResultsChanged;

    [SerializeField]
    private PatientSymptomManager patientSymptomManager = null;

    [SerializeField]
    private List<IngredientSlot> ingredientSlots = new List<IngredientSlot>();

    [SerializeField]
    public List<ExperimentResult> experimentResults = new List<ExperimentResult>();

    [SerializeField]
    private int patientsToGo = 10;
    private int PatientsToGo { get { return patientsToGo; } set { patientsToGo = value; OnPatientsToGoChanged?.Invoke(patientsToGo); } }

    private int patientsCured = 0;
    private int PatientsCured { get { return patientsCured; } set { patientsCured = value; OnPatientsCuredChanged?.Invoke(patientsCured); } }

    private bool resolvingPotion = false;

    private void Start()
    {
        patientSymptomManager = new PatientSymptomManager();
    }

    private void DebugTestPotion(Potion potion, int iterations)
    {
        for (int i = 0; i < iterations; ++i)
        {
            patientSymptomManager.ResetPatient();
            ApplyPotion(potion);
        }

        Debug.Log("Score: " + patientsCured);
        foreach (ExperimentResult result in experimentResults)
        {
            Debug.Log("Before: " + Helpers.GetDebugStringFromHashSet(result.symptomsBefore)
                + " - After: " + Helpers.GetDebugStringFromHashSet(result.symptomsAfter));
        }
    }

    /// <summary>
    /// Called when brew button is clicked. Waits until OnPatientFinished is called before it is able to be called again.
    /// </summary>
    public void OnBrewPotion()
    {
        List<Ingredient> ingredientList = GetIngredients();
        ApplyPotion(new Potion(ingredientList));
    }

    /// <summary>
    /// Called when patient is finished animating away.
    /// </summary>
    public void OnPatientFinished()
    {
        PatientsToGo -= 1;

        if (PatientsToGo <= 0)
        {
            // End game and send score
            OnGameEnd?.Invoke(patientsCured);
        }
        else
        {
            // Next patient
            patientSymptomManager.ResetPatient();
            resolvingPotion = false;
        }
    }

    private List<Ingredient> GetIngredients()
    {
        List<Ingredient> ingredientList = new List<Ingredient>();
        foreach (IngredientSlot ingredientSlot in ingredientSlots)
        {
            ingredientList.Add(ingredientSlot.ingredient);
        }

        return new List<Ingredient>();
    }

    private void ApplyPotion(Potion potion)
    {
        if (!resolvingPotion)
        {
            resolvingPotion = true;

            // Store the patients symptoms for later
            HashSet<eSymptom> symptomsBefore = new HashSet<eSymptom>(patientSymptomManager.symptoms);

            // Apply potion to patient and score accordingly
            bool isPatientCured = patientSymptomManager.ApplyPotionToPatient(potion.GetSymptomChange());
            patientsCured += isPatientCured ? 1 : 0;

            // Log experiment results
            HashSet<eSymptom> symptomsAfter = new HashSet<eSymptom>(patientSymptomManager.symptoms);
            experimentResults.Add(new ExperimentResult(symptomsBefore, potion.PotionComposition, symptomsAfter));
            ExperimentResultsChanged?.Invoke(experimentResults);
        }
        else
        {
            Debug.Log("OnPatientFinished must be called before another potion can be applied!");
        }
    }
}
