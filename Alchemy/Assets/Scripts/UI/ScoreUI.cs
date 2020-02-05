using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI patientsText;

    void Start()
    {
        GameManager gm = GameManager.Instance;

        patientsText.text = gm.PatientsToGo.ToString();
        scoreText.text = "0";

        gm.OnPatientsToGoChanged += OnPatientsToGoChangedListener;
        gm.OnPatientsCuredChanged += OnPatientsCuredChangedListener;
    }

    void OnPatientsToGoChangedListener(int patientsToGo)
    {
        patientsText.text = patientsToGo.ToString();
    }

    void OnPatientsCuredChangedListener(int patientsCured)
    {
        scoreText.text = patientsCured.ToString();
    }
}
