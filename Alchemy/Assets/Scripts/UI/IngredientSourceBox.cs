using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientSourceBox : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameUIManager gameUIManager;

    public Image image;

    //Temporary, for testing. Should be set by the GameUIManager.
    void Start()
    {
        Ingredient = IngredientMgr.Instance.ingredientTypes[0];
    }

    private Ingredient ingredient;
    public Ingredient Ingredient
    {
        get => ingredient;
        set
        {
            ingredient = value;
            image.sprite = ingredient?.View.Sprite;
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        gameUIManager.DraggedIngredient = Ingredient;
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameUIManager.MoveDraggedIngredient(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameUIManager.DraggedIngredient == Ingredient)
            gameUIManager.DraggedIngredient = null;
    }
}
