using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEntry
{
	readonly List<Ingredient> PotionIngredients;
	readonly Potion ResultingPotion;
	readonly HashSet<eSymptom> ChangeInSymptoms;

	public LogEntry(List<Ingredient> ingredients, HashSet<eSymptom> symptomChange)
	{
		PotionIngredients = ingredients;
		ResultingPotion = new Potion( PotionIngredients );
		ChangeInSymptoms = symptomChange;
	}
}
