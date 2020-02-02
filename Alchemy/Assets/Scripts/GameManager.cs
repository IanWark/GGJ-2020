using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    // Sends the new number of patients cured when invoked
    public event Action<int> OnPatientsCuredChanged;
    // Sends the new number of patients left when invoked
    public event Action<int> OnPatientsToGoChanged;
    // Sends the number of patients cured at the end of the game, and total patients
    public event Action<int, int> OnGameEnd;
    // Sends the list of potion experiment results
    public event Action<List<ExperimentResult>> ExperimentResultsChanged;
    // Sends the slot index and ingredient when an ingredient slot changes (can send null) 
    public event Action<int, Ingredient> OnIngredientChange;
    // Send a bool when the can brew state changes
    public event Action<bool> OnCanBrewStateChange;

    [SerializeField]
    private PatientSymptomManager patientSymptomManager = null;

    public Book bookData = null;

    private Ingredient[] ingredients = new Ingredient[3];

    public List<ExperimentResult> experimentResults = new List<ExperimentResult>();

    [SerializeField]
    private int patientsTotal = 10;

    private int patientsToGo = -1;
    public int PatientsToGo
    {
        get { return patientsToGo; }
        set { patientsToGo = value; OnPatientsToGoChanged?.Invoke(patientsToGo); }
    }

    private int patientsCured = 0;
    private int PatientsCured
    {
        get { return patientsCured; }
        set { patientsCured = value; OnPatientsCuredChanged?.Invoke(patientsCured); }
    }

    private bool resolvingPotion = false;
    private bool setFirstResultsAlready = false;
    
    public int NumberOfIngredients
    {
        get
        {
            int sum = 0;
            for (int i = 0; i < ingredients.Length; i++)
            {
                if (ingredients[i] != null)
                    sum++;
            }
            return sum;
        }
    }

    public bool CanBrew => NumberOfIngredients >= 2 && !resolvingPotion;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bookData = new Book(GameManager.Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        PatientsToGo = patientsTotal;

        patientSymptomManager = new PatientSymptomManager();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        if (!setFirstResultsAlready)
        {
            setFirstResultsAlready = true;

            Potion potion1 = new Potion(new List<Ingredient>()
            {
                IngredientMgr.Instance.ingredientTypes[0],
                IngredientMgr.Instance.ingredientTypes[2]
            });
            AddStartingExperimentalData(potion1, 3);
        }
    }

    private void AddStartingExperimentalData(Potion potion, int iterations)
    {
        ApplyPotion(potion, false);

        for (int i = 0; i < iterations - 1; ++i)
        {
            OnPatientFinished(false);
            ApplyPotion(potion, false);
        }

        OnPatientFinished(false);
        
        foreach (ExperimentResult result in experimentResults)
        {
            Debug.Log("Before: " + Helpers.GetDebugStringFromHashSet(result.symptomsBefore)
                + " - After: " + Helpers.GetDebugStringFromHashSet(result.symptomsAfter));
        }
    }

    /// <summary>
    /// Called when brew button is clicked. Waits until OnPatientFinished is called before it is able to be called again.
    /// </summary>
    public void BrewPotion()
    {
        List<Ingredient> ingredientList = GetIngredients();
        ApplyPotion(new Potion(ingredientList), true);
    }

    /// <summary>
    /// Called when patient is finished animating away.
    /// </summary>
    public void OnPatientFinished(bool reducePatients)
    {
        PatientsToGo -= reducePatients ? 1 : 0;

        if (PatientsToGo <= 0)
        {
            // End game and send score
            OnGameEnd?.Invoke(patientsCured, patientsTotal);
        }
        else
        {
            // Next patient
            patientSymptomManager.ResetPatient();
            resolvingPotion = false;
            OnCanBrewStateChange?.Invoke(CanBrew);
        }
    }

    private List<Ingredient> GetIngredients()
    {
        List<Ingredient> ingredientList = new List<Ingredient>();
        foreach (var ingredient in ingredients)
        {
            if (ingredient != null)
            {
                ingredientList.Add(ingredient);
            }
        }

        return ingredientList;
    }

    public void SetIngredient(int index, Ingredient ingredient)
    {
        ingredients[index] = ingredient;
        OnIngredientChange?.Invoke(index, ingredient);
        OnCanBrewStateChange?.Invoke(CanBrew);
    }

    public Ingredient GetIngredient(int index)
    {
        return ingredients[index];
    }

    private void ApplyPotion(Potion potion, bool addScoreOnSuccess)
    {
        if (!resolvingPotion)
        {
            resolvingPotion = true;
            OnCanBrewStateChange?.Invoke(CanBrew);

            // Store the patients symptoms for later
            HashSet<eSymptom> symptomsBefore = new HashSet<eSymptom>(patientSymptomManager.symptoms);

            // Apply potion to patient and score accordingly
            bool isPatientCured = patientSymptomManager.ApplyPotionToPatient(potion.GetSymptomChange());
            patientsCured += (addScoreOnSuccess && isPatientCured) ? 1 : 0;

            // Log experiment results
            HashSet<eSymptom> symptomsAfter = new HashSet<eSymptom>(patientSymptomManager.symptoms);
            ExperimentResult newResult = new ExperimentResult(symptomsBefore, potion.PotionComposition, symptomsAfter);
            experimentResults.Add(newResult);
            ExperimentResultsChanged?.Invoke(experimentResults);
        }
        else
        {
            Debug.Log("OnPatientFinished must be called before another potion can be applied!");
        }
    }
}
