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
        if (AudioManager.Instance != null) AudioManager.Instance.PlayBrew();

        GameManager.Instance.BrewPotion();

        // TODO shouldn't happen immediately, instead should happen after any animations finish
        GameManager.Instance.OnPatientFinished(true);
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
