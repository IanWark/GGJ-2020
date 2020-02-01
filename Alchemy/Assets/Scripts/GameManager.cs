﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Sends the new number of patients cured when invoked
    public event Action<int> OnPatientsCuredChanged;
    // Sends the new number of patients left when invoked
    public event Action<int> OnPatientsLeftChanged;
    // Sends the number of patients cured at the end of the game
    public event Action<int> OnGameEnd;
    // Sends the list of potion experiment results
    public event Action<List<ExperimentResult>> ExperimentResultsChanged;

    [SerializeField]
    private PatientSymptomManager patientManager = null;

    //[SerializeField]
    //private List<IngredientSlot> ingredientSlots = new List<IngredientSlot>();

    public List<ExperimentResult> experimentResults = new List<ExperimentResult>();

    private int patientsLeft = 0;
    private int PatientsToGo { get { return patientsLeft; } set { patientsLeft = value; OnPatientsLeftChanged?.Invoke(patientsLeft); } }

    private int patientsCured = 0;
    private int PatientsCured { get { return patientsCured; } set { patientsCured = value; OnPatientsCuredChanged?.Invoke(patientsCured); } }

    private void Start()
    {
        patientManager = new PatientSymptomManager();
    }

    /// <summary>
    /// Called when brew button is clicked.
    /// </summary>
    public void OnBrewPotion()
    {
        // Store the patients symptoms for the log later
        HashSet<eSymptom> symptomsBefore = patientManager.symptoms;

        // Brew and apply the potion
        List<Ingredient> ingredientList = GetIngredients();
        List<SymptomChange> symptomChanges = GetResultsOfPotion(ingredientList);
        ApplyChanges(symptomChanges);

        // Log experiment results
        HashSet<eSymptom> symptomsAfter = patientManager.symptoms;
        experimentResults.Add(new ExperimentResult(symptomsBefore, ingredientList, symptomsAfter));
        ExperimentResultsChanged?.Invoke(experimentResults);
    }

    // TODO actually get ingredients
    private List<Ingredient> GetIngredients()
    {
        return new List<Ingredient>();
    }

    // TODO actually send to potion resolving class
    private List<SymptomChange> GetResultsOfPotion(List<Ingredient> ingredients)
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

    private void PatientFinished()
    {
        PatientsToGo -= 1;
        
        if(PatientsToGo <= 0)
        {
            // End game and send score
            OnGameEnd?.Invoke(patientsCured);
        }
        else
        {
            // Next patient
            patientManager.ResetPatient();
        }
    }
}
