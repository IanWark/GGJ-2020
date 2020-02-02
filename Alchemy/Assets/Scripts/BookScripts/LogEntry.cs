using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEntry
{
	static public bool IsSameLogEntry(LogEntry entry1, LogEntry entry2)
	{
		foreach(KeyValuePair<string,int> kvp in entry1.PotionIngredients)
		{
			if(!(entry2.PotionIngredients.ContainsKey(kvp.Key) && (entry2.PotionIngredients[kvp.Key]==kvp.Value)))
				return false;
		}
		return true;
	}

	public class SymptomChangeStatus
	{
		public SymptomChange Symptom;
		public bool IsRevealled;

		public SymptomChangeStatus(SymptomChange sc)
		{
			Symptom = sc;
			IsRevealled = false;
		}
	}
	
	public readonly Dictionary<string,int> PotionIngredients;
	public readonly List<SymptomChangeStatus> PotionEffects;

	public LogEntry(List<Ingredient> ingredients)
	{
		foreach(Ingredient ingre in ingredients)
		{
			if(PotionIngredients.ContainsKey(ingre.View.Name))
			{
				PotionIngredients[ingre.View.Name] += 1;
			}
			else
			{
				PotionIngredients.Add(ingre.View.Name, 1);
			}
		}
		Potion resultingPotion = new Potion( ingredients );
		List<SymptomChange> scList = resultingPotion.GetSymptomChange();
		foreach(SymptomChange sc in scList)
		{
			PotionEffects.Add( new SymptomChangeStatus(sc) );
		}
	}

	public void UpdateEntry(eSymptom symp)
	{
		foreach(SymptomChangeStatus scs in PotionEffects)
		{
			if(scs.Symptom.symptom == symp)
			{ 
				scs.IsRevealled = true;
				return;
			}
		}
	}
}
