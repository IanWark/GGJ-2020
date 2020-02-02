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

	public event Action OnLogEntryListChanged;

	private List<LogEntry> m_LogEntryList;
	public List<LogEntry> LogEntryList { get => m_LogEntryList; }

	public Book( GameManager gm )
	{
		GameManagerRef = gm;
		GameManagerRef.OnPatientsCuredChanged += OnPatientCuredChangedHandler;
		GameManagerRef.OnPatientsToGoChanged += OnPatientsLeftChangedHandler;
		GameManagerRef.ExperimentResultsChanged += ExperimentResultChangedHandler;

		m_LogEntryList = new List<LogEntry>();
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

	private HashSet<eSymptom> ChangeInSymptoms(HashSet<eSymptom> before, HashSet<eSymptom> after)
	{
		HashSet<eSymptom> difference = new HashSet<eSymptom>(before);

		difference.SymmetricExceptWith(after);

		return difference;
	}
	private void ExperimentResultChangedHandler(List<ExperimentResult> erList)
	{
		foreach(ExperimentResult er in erList)
		{
			HashSet<eSymptom> difference = ChangeInSymptoms(er.symptomsBefore, er.symptomsAfter);
			if(difference.Count > 0)
			{
				LogEntry tmpEntry = new LogEntry(er.ingredientList);
				LogEntry searchEntry = LogEntryList.Find( (element) => { return LogEntry.IsSameLogEntry(tmpEntry, element); } );
				
				if( searchEntry == null ) // this log entry does not exist yet
				{ 
					LogEntryList.Add(tmpEntry);
					searchEntry = tmpEntry;
				}

				foreach (eSymptom symp in difference)
				{
					searchEntry.UpdateEntry(symp);
				}
			}
		}

		OnLogEntryListChanged?.Invoke();
	}
}
