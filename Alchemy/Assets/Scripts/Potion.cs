using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
	readonly public List<Ingredient> PotionComposition;

	public Potion(List<Ingredient> composition)
	{
		PotionComposition = composition;
	}

	public List<SymptomChange> GetSymptomChange()
	{
		List<SymptomChange> result = new List<SymptomChange>();

		foreach(Ingredient ingre in PotionComposition)
		{
			foreach(SymptomChange scFromComposition in ingre.SymptomChanges.symptomChanges)
			{
				SymptomChange searchResult = result.Find( (element) => { return element.symptom == scFromComposition.symptom; } );
				if( searchResult == null)
				{
					result.Add( new SymptomChange( scFromComposition.symptom, scFromComposition.change ) );
				}
				else
				{
					searchResult.change += scFromComposition.change;
				}
			}
		}

		return result;
	}
}
