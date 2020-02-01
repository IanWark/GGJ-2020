using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public Action OnPatientReset;

    private eSymptom symptom = eSymptom.sympA;
    public eSymptom Symptom { get; }

    private int numberOfSymptomsPossible = Enum.GetValues(typeof(eSymptom)).Length;

    private void Start()
    {
        ResetPatient();
    }

    void ResetPatient()
    {
        // Randomly generate a symptom
        symptom = (eSymptom) UnityEngine.Random.Range(0, numberOfSymptomsPossible);

        // Send event to UI
        OnPatientReset?.Invoke();
    }
}
