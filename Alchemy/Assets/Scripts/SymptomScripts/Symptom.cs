using System.Collections.Generic;
using UnityEngine;

public enum eSymptom
{
    GreenFace = 0,
    Wart = 1,
    Horn = 2,
    Frog = 3,
}

[System.Serializable]
public class SymptomChange 
{
    public eSymptom symptom = eSymptom.GreenFace;
    public int change = 0;

    public SymptomChange(eSymptom symptom, int change)
    {
        this.symptom = symptom;
        this.change = change;
    }
}

// Unity's inspector doesn't do lists of lists by default, so wrapping it in a class allows that
[System.Serializable]
public class SymptomChangeList
{
    [SerializeField]
    private string id = string.Empty;
    public string ID { get { return id; } }

    [SerializeField]
    public List<SymptomChange> symptomChanges = new List<SymptomChange>();
}