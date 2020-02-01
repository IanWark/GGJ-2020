using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
	private List<Ingredient> PotionComposition;

	public Potion(List<Ingredient> composition)
	{
		PotionComposition = composition;
	}

	public List<SymptomChange> GetSymptomChange()
	{
		List<SymptomChange> result = new List<SymptomChange>();

		foreach(Ingredient ingre in PotionComposition)
		{
			foreach(SymptomChange sc in ingre.SymptomChanges)
			{
				if(result.Find( (element) => { return element.Id == sc.Id; } ) == null)
				{

				}
				else
				{

				}
			}
		}

		return result;
	}
}
