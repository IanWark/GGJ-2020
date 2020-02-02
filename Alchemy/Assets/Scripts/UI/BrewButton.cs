using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrewButton : MonoBehaviour
{
    [SerializeField]
    public Button button = null;

    [SerializeField]
    private Image brewTextImage = null;

    [SerializeField]
    private Image gooImage = null;

    public void SetCanBrew(bool canBrew)
    {
        button.interactable = canBrew;
        brewTextImage.enabled = canBrew;
        gooImage.enabled = canBrew;
    }
}
