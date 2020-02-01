using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientTargetBox : MonoBehaviour, IDropHandler
{
    public GameUIManager gameUIManager;

    public int ingredientIndex;

    public void OnDrop(PointerEventData eventData)
    {
        if (gameUIManager.DraggedIngredient != null)
        {
            gameUIManager.SetIngredient(ingredientIndex, gameUIManager.DraggedIngredient);
        }
    }
}
