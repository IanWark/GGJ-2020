using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSymptomManager
{
    // Send what the new set of symptoms are when invoked
    public event Action<HashSet<eSymptom>> OnNewPatient;
    // Send what the new set of symptoms are when invoked
    public event Action<HashSet<eSymptom>> OnSymptomsChanged;

    public HashSet<eSymptom> symptoms = new HashSet<eSymptom>();

    private int numberOfSymptomsPossible = Enum.GetValues(typeof(eSymptom)).Length;
    private eSymptom lastRandomSymptom = eSymptom.sympC;

    public PatientSymptomManager()
    {
        ResetPatient();
    }

    /// <summary>
    /// Applies potion to patient, and returns whether they were successfully cured.
    /// </summary>
    /// <returns>If patient was successfully cured.</returns>
    public bool ApplyPotionToPatient(List<SymptomChange> changeList)
    {
        foreach (SymptomChange change in changeList)
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
        eSymptom randSymptom = (eSymptom)Helpers.RandomIntExceptOne(0, numberOfSymptomsPossible, (int)lastRandomSymptom);

        symptoms.Add(randSymptom);

        lastRandomSymptom = randSymptom;

        // Send event to UI
        OnNewPatient?.Invoke(symptoms);
    }

    public void DebugPrintSymptoms()
    {
        Debug.Log(String.Join(",", symptoms));
    }
}
