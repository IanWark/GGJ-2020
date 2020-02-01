using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public Action<HashSet<eSymptom>> OnSymptomsChanged;

    private HashSet<eSymptom> symptoms = new HashSet<eSymptom>();

    private int numberOfSymptomsPossible = Enum.GetValues(typeof(eSymptom)).Length;

    private void Start()
    {
        ResetPatient();
    }

    public void ResetPatient()
    {
        // Randomly generate a symptom
        symptoms.Clear();
        eSymptom newSymptom = (eSymptom) UnityEngine.Random.Range(0, numberOfSymptomsPossible);

        ApplySymptom(newSymptom);
    }

    public void ApplySymptom(eSymptom symptom)
    {
        symptoms.Add(symptom);

        // Send event to UI
        OnSymptomsChanged?.Invoke(symptoms);
    }

    public void RemoveSymptom(eSymptom symptom)
    {
        symptoms.Remove(symptom);

        // Send event to UI
        OnSymptomsChanged?.Invoke(symptoms);
    }
}
