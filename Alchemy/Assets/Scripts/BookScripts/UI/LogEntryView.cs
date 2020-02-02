using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogEntryView : MonoBehaviour
{
    [SerializeField]
    public List<Image> ingredientImages = null;

    [SerializeField]
    public TextMeshProUGUI resultsText = null;

    public void SetLogEntry(LogEntry entry)
    {
        // Set images, 1 for each ingredient
        int imageIndex = 0;
        foreach (KeyValuePair<Ingredient, int> ingredientPair in entry.PotionIngredients)
        {
            for (int i = 0; i < ingredientPair.Value; ++i)
            {
                ingredientImages[imageIndex].sprite = ingredientPair.Key.View.Sprite;
                ingredientImages[imageIndex].enabled = true;

                ++imageIndex;            
            }
        }

        for (int i = imageIndex; i < ingredientImages.Count; ++i)
        {
            ingredientImages[imageIndex].enabled = false;
        }

        // Create a string depicting the results of the combination
        string resultsString = string.Empty;
        foreach (LogEntry.SymptomChangeStatus scs in entry.PotionEffects)
        {
            if (scs.IsRevealled)
            {
                if (scs.Symptom.change > 0)
                {
                    resultsString += "+";
                }
                else if (scs.Symptom.change < 0)
                {
                    resultsString += "-";
                }
                resultsString += scs.Symptom.symptom + "\n";
            }
        }
        if (resultsString.Equals(string.Empty))
        {
            resultsString += "Nothing";
        }

        resultsText.text = resultsString;
    }
}
