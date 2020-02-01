using System.Collections.Generic;
using UnityEngine;

public enum eSymptom
{
    sympA = 0,
    sympB = 1,
    sympC = 2,
}

[System.Serializable]
public class SymptomChange 
{
    public eSymptom symptom = eSymptom.sympA;
    public int change = 0;
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
    public List<SymptomChange> SymptomChanges { get; }
}