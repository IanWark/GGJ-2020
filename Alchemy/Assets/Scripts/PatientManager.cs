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

    /// <summary>
    /// Applies potion to patient, and returns whether they were a success
    /// </summary>
    /// <returns>If patient was successfully cured.</returns>
    public bool ApplyPotionToPatient(SymptomChangeList changeList)
    {
        foreach (SymptomChange change in changeList.SymptomChanges)
        {
            if (change.change > 0)
            {
                symptoms.Add(change.symptom);
            }
            else if (change.change < 0)
            {
                symptoms.Remove(change.symptom);
            }
        }

        OnSymptomsChanged?.Invoke(symptoms);

        return symptoms.Count == 0;
    }

    public void ResetPatient()
    {
        // Randomly generate a symptom
        symptoms.Clear();
        eSymptom randSymptom = (eSymptom) UnityEngine.Random.Range(0, numberOfSymptomsPossible);

        symptoms.Add(randSymptom);

        // Send event to UI
        OnSymptomsChanged?.Invoke(symptoms);
    }
}
