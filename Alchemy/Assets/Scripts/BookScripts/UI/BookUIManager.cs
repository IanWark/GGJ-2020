using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelHolder = null;

    [SerializeField]
    private GameObject screen1Parent = null;

    [SerializeField]
    private GameObject screen2Parent = null;

    [SerializeField]
    private Button toScreen1Button = null;

    [SerializeField]
    private Button toScreen2Button = null;

    [SerializeField]
    private Button closeButton = null;

    private bool isOpen = false;
    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; panelHolder.SetActive(isOpen); }
    } 

    private void Start()
    {
        isOpen = false;
        ChangeScreen(true);

        toScreen1Button.onClick.AddListener(OnPage1ButtonClicked);
        toScreen2Button.onClick.AddListener(OnPage2ButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnDestroy()
    {
        toScreen1Button.onClick.RemoveListener(OnPage1ButtonClicked);
        toScreen2Button.onClick.RemoveListener(OnPage2ButtonClicked);
    }

    private void ChangeScreen(bool isPage1)
    {
        screen1Parent.SetActive(isPage1);
        screen2Parent.SetActive(!isPage1);
    }

    private void OnPage1ButtonClicked()
    {
        ChangeScreen(true);
    }

    private void OnPage2ButtonClicked()
    {
        ChangeScreen(false);
    }

    private void OnCloseButtonClicked()
    {
        IsOpen = false;
    }
}
