using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentResult
{
    public HashSet<eSymptom> symptomsBefore = null;
    public List<Ingredient> ingredientList = null;
    public HashSet<eSymptom> symptomsAfter = null;

    public ExperimentResult(HashSet<eSymptom> symptomsBefore, List<Ingredient> ingredientList, HashSet<eSymptom> symptomsAfter)
    {
        this.symptomsBefore = symptomsBefore;
        this.ingredientList = ingredientList;
        this.symptomsAfter = symptomsAfter;
    }
}
