using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private BrewButton brewButton;

    [SerializeField]
    private Button bookButton = null;

    [SerializeField]
    private BookUIManager bookUIManager = null;

    [SerializeField]
    private PatientImage patientImage = null;

    [SerializeField]
    private GameObject ingredientSourcePrefab = null;

    [SerializeField]
    private Transform ingredientContainer = null;

    private Ingredient draggedIngredient;
    public Ingredient DraggedIngredient
    {
        get => draggedIngredient;
        set
        {
            if (draggedIngredient != value)
            {
                draggedIngredient = value;
                SetDragIngredientSprite(draggedIngredient);
            }
        }
    }

    public Image draggedIngredientImage;

    public RectTransform draggedIngredientRectTransform;

    void Start()
    {
        brewButton.button.onClick.AddListener(OnBrewButtonClick);
        bookButton.onClick.AddListener(OnBookButtonClick);

        GameManager.Instance.OnCanBrewStateChange += OnCanBrewChangeListener;
        OnCanBrewChangeListener(GameManager.Instance.CanBrew);

        SetupIngredientList();
    }

    private void SetupIngredientList()
    {
        foreach (var item in IngredientMgr.Instance.ingredientTypes)
        {
            GameObject go = Instantiate<GameObject>(ingredientSourcePrefab, ingredientContainer);
            IngredientSourceBox sb = go.GetComponent<IngredientSourceBox>();
            sb.Ingredient = item;
            sb.gameUIManager = this;
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCanBrewStateChange -= OnCanBrewChangeListener;
    }

    public void MoveDraggedIngredient(Vector2 mousePosition)
    {
        draggedIngredientRectTransform.position = mousePosition;
    }

    void SetDragIngredientSprite(Ingredient ingredient)
    {
        if (ingredient != null)
        {
            draggedIngredientRectTransform.gameObject.SetActive(true);
            draggedIngredientImage.sprite = ingredient.View.Sprite;
        }
        else
        {
            draggedIngredientRectTransform.gameObject.SetActive(false);
            draggedIngredientImage.sprite = null;
        }
    }

    public void SetIngredient(int index, Ingredient ingredient)
    {
        GameManager.Instance.SetIngredient(index, ingredient);
        Debug.Log($"Ingredient {index} set to {ingredient?.View.Name}");
    }

    public void OnBrewButtonClick()
    {
        StartCoroutine(BrewPotionCoroutine());
    }

    public IEnumerator BrewPotionCoroutine()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayBrew();

        // Show Smoke
        patientImage.ShowSmoke(true);

        yield return new WaitForSeconds(0.5f);

        // Apply potion
        GameManager.Instance.BrewPotion();
        patientImage.ShowSmoke(false);

        // Move out
        patientImage.AnimateOut();

        yield return new WaitForSeconds(0.5f);

        // Get new patient and move in
        GameManager.Instance.OnPatientFinished(true);
        patientImage.AnimateIn();
    }

    private void OnCanBrewChangeListener(bool canBrew)
    {
        brewButton.SetCanBrew(canBrew);
    }

    private void OnBookButtonClick()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayOpenBook();

        bookUIManager.IsOpen = true;
    }
}
