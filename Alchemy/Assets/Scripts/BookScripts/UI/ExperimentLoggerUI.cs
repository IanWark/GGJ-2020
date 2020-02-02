using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentLoggerUI : MonoBehaviour
{
    [SerializeField]
    private Transform logEntryPrefab = null;

    [SerializeField]
    private Transform entryHolder = null;

    private List<LogEntryView> viewsList = new List<LogEntryView>(); 

    private void Start()
    {
        GameManager.Instance.bookData.OnLogEntryListChanged += UpdateEntries;
    }

    private void UpdateEntries(List<LogEntry> entryList)
    {
        // Make sure there are enough LogEntryViews first
        if (entryList.Count > viewsList.Count)
        {
            for (int i = viewsList.Count; i < entryList.Count; ++i)
            {
                Transform newObject = Instantiate(logEntryPrefab, entryHolder);

                LogEntryView logEntryView = newObject.GetComponent<LogEntryView>();
                viewsList.Add(logEntryView);
            }
        }

        // Update each entry to match entryList
        for (int i = 0; i < entryList.Count; ++i) {      
            viewsList[i].SetLogEntry(entryList[i]);
        }
    }
}
