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

    public event Action<List<LogEntry>> OnLogEntryListChanged;

    private List<LogEntry> m_LogEntryList;
    public List<LogEntry> LogEntryList { get => m_LogEntryList; }

    public Book(GameManager gm)
    {
        GameManagerRef = gm;
        GameManagerRef.OnPatientsCuredChanged += OnPatientCuredChangedHandler;
        GameManagerRef.OnPatientsToGoChanged += OnPatientsLeftChangedHandler;
        GameManagerRef.ExperimentResultsChanged += ExperimentResultChangedHandler;

        m_LogEntryList = new List<LogEntry>();
    }
    ~Book()
    {
        if (GameManagerRef != null)
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
        LogEntry searchEntry = null;
		bool IsLogChanged = false;

		foreach (ExperimentResult er in erList)
        {
			LogEntry tmpEntry = new LogEntry(er.ingredientList);

			searchEntry = LogEntryList.Find((element) => { return LogEntry.IsSameLogEntry(tmpEntry, element); });
			if (searchEntry == null) // this log entry does not exist yet
			{
				LogEntryList.Add(tmpEntry);
				searchEntry = tmpEntry;
				IsLogChanged |= true;
			}

			HashSet<eSymptom> difference = ChangeInSymptoms(er.symptomsBefore, er.symptomsAfter);
            if (difference.Count > 0)
            {
                foreach (eSymptom symp in difference)
                {
                    IsLogChanged |= searchEntry.UpdateEntry(symp);
                }
            }
        }

        if(IsLogChanged)
			OnLogEntryListChanged?.Invoke(LogEntryList);
    }
}
