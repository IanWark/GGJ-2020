using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book
{
    // Rewires patient cured event from game manager
    public event Action<int> OnPatientsCuredChangedFromGame;
    // Rewires patient cured event from game manager
    public event Action<int> OnPatientsToGoChangedFromGame;

	public List<LogEntry> LogEntryList;

	public Book( GameManager gm )
	{
		GameManagerRef = gm;
		GameManagerRef.OnPatientsCuredChanged += OnPatientCuredChangedHandler;
		GameManagerRef.OnPatientsToGoChanged += OnPatientsLeftChangedHandler;
		GameManagerRef.ExperimentResultsChanged += ExperimentResultChangedHandler;

		LogEntryList = new List<LogEntry>();
	}
	~Book()
	{
		if( GameManagerRef != null )
		{
			GameManagerRef.OnPatientsCuredChanged -= OnPatientCuredChangedHandler;
			GameManagerRef.OnPatientsToGoChanged -= OnPatientsLeftChangedHandler;
			GameManagerRef.ExperimentResultsChanged -= ExperimentResultChangedHandler;
		}
	}

	private readonly GameManager GameManagerRef;
	
	private void OnPatientCuredChangedHandler(int patientsCured)
	{ OnPatientsCuredChangedFromGame?.Invoke(patientsCured); }
	private void OnPatientsLeftChangedHandler(int patientLeft)
	{ OnPatientsToGoChangedFromGame?.Invoke(patientLeft); }

	// Implementation is inefficient right now.
	// Clear all log entries and go through the entire list of experiment results again.
	private void ExperimentResultChangedHandler(List<ExperimentResult> erList)
	{
		LogEntryList.Clear();

		HashSet<eSymptom> ChangeInSymptoms = new HashSet<eSymptom>();
		foreach(ExperimentResult er in erList)
		{
			foreach(eSymptom symptom in er.symptomsAfter)
			{
				if(!er.symptomsBefore.Contains(symptom))
					ChangeInSymptoms.Add(symptom);
			}
			if(ChangeInSymptoms.Count > 0)
			{
				LogEntryList.Add(new LogEntry(er.ingredientList, ChangeInSymptoms));
				ChangeInSymptoms = new HashSet<eSymptom>();
			}
		}
	}
}
