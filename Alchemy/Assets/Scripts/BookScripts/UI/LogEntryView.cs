using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogEntryView : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI ingredientsText = null;

    [SerializeField]
    public TextMeshProUGUI resultsText = null;

    public void SetLogEntry(string ingredientsString, string resultsString)
    {
        ingredientsText.text = ingredientsString;
        resultsText.text = resultsString;
    }
}
