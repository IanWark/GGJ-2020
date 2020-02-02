using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientTargetBox : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public GameUIManager gameUIManager;

    public Image ingredientImage;

    public int ingredientIndex;

    void Start()
    {
        GameManager.Instance.OnIngredientChange += OnIngredientSlotChangeListener;
        SetIngredient(GameManager.Instance.GetIngredient(ingredientIndex));
    }

    void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnIngredientChange -= OnIngredientSlotChangeListener;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (gameUIManager.DraggedIngredient != null)
        {
            gameUIManager.SetIngredient(ingredientIndex, gameUIManager.DraggedIngredient);
        }
    }

    void SetIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            ingredientImage.enabled = false;
            ingredientImage.sprite = null;
        }
        else
        {
            AudioManager.Instance.PlaySlotIngredientClip();
			ingredientImage.sprite = ingredient.View.Sprite;
            ingredientImage.enabled = true;
        }
    }

    void OnIngredientSlotChangeListener(int index, Ingredient ingredient)
    {
        if (index == ingredientIndex)
        {
            SetIngredient(ingredient);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameUIManager.SetIngredient(ingredientIndex, null);
    }
}
