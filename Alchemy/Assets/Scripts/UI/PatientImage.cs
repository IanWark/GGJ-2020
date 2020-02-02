using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientImage : MonoBehaviour
{
	// If there is a better way to handle the following block of code, go crazy with it
	public UnityEngine.UI.Image BaseImageComponent = null;
	public UnityEngine.UI.Image WartAddOnComponent = null;
	public UnityEngine.UI.Image HornAddOnComponent = null;
	public Sprite HappyPatientSprite = null;
	public Sprite SadPatientSprite = null;
	public Sprite GreenFacedPatientSprite = null;
	public Sprite HornAddOnSprite = null;
	public Sprite WartAddOnSprite = null;
	// Craziness ends here

	private PatientSymptomManager Psm;

	private void CheckAssets()
	{
		if( !(BaseImageComponent&&
			  WartAddOnComponent&&
			  HornAddOnComponent&&
			  HappyPatientSprite&&
			  SadPatientSprite&&
			  GreenFacedPatientSprite&&
			  HornAddOnSprite&&
			  WartAddOnSprite) )
			Debug.Log("Assets in editor are never configured for PatientImage script.");
	}

	private void Init()
	{
		BaseImageComponent.sprite = HappyPatientSprite;

		WartAddOnComponent.sprite = WartAddOnSprite;
		WartAddOnComponent.enabled = false;

		HornAddOnComponent.sprite = HornAddOnSprite;
		HornAddOnComponent.enabled = false;

		SwitchSprite(Psm.symptoms);
	}

	// Start is called before the first frame update
    void Start()
    {
        CheckAssets();

		Psm = new PatientSymptomManager();
		Psm.OnNewPatient += OnNewPatientHander;
		Psm.OnSymptomsChanged += OnSymptomsChangedHandler;
		Init();
    }

	private void OnDestroy()
	{
		Psm.OnNewPatient -= OnNewPatientHander;
		Psm.OnSymptomsChanged -= OnSymptomsChangedHandler;
	}

	private void OnNewPatientHander(HashSet<eSymptom> symptoms)
	{ SwitchSprite(symptoms); }
	private void OnSymptomsChangedHandler(HashSet<eSymptom> symptoms)
	{ SwitchSprite(symptoms); }

	private void SwitchSprite(HashSet<eSymptom> symptoms)
	{
		int PatientImageState = 0;
		foreach(eSymptom symp in symptoms)
		{
			PatientImageState |= (1 << (int)symp);
		}

		switch(PatientImageState)
		{
			case 0: // happy patient
				BaseImageComponent.sprite = HappyPatientSprite;
				WartAddOnComponent.enabled = false;
				HornAddOnComponent.enabled = false;
				break;
			case 1: // green-faced patient
				BaseImageComponent.sprite = GreenFacedPatientSprite;
				WartAddOnComponent.enabled = false;
				HornAddOnComponent.enabled = false;
				break;
			case 2: // patient with wart
				BaseImageComponent.sprite = SadPatientSprite;
				WartAddOnComponent.enabled = true;
				HornAddOnComponent.enabled = false;
				break;
			case 3: // green-faced patient with wart
				BaseImageComponent.sprite = GreenFacedPatientSprite;
				WartAddOnComponent.enabled = true;
				HornAddOnComponent.enabled = false;
				break;
			case 4: // patient with horn
				BaseImageComponent.sprite = SadPatientSprite;
				WartAddOnComponent.enabled = false;
				HornAddOnComponent.enabled = true;
				break;
			case 5: // green-faced patient with horn
				BaseImageComponent.sprite = GreenFacedPatientSprite;
				WartAddOnComponent.enabled = false;
				HornAddOnComponent.enabled = true;
				break;
			case 6: // patient with wart and horn
				BaseImageComponent.sprite = SadPatientSprite;
				WartAddOnComponent.enabled = true;
				HornAddOnComponent.enabled = true;
				break;
			case 7: // green-faced patient with wart and horn
				BaseImageComponent.sprite = GreenFacedPatientSprite; // + wart and horn add ons
				WartAddOnComponent.enabled = true;
				HornAddOnComponent.enabled = true;
				break;
		}
	}
}
