using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientImage : MonoBehaviour
{
	// If there is a better way to handle the following block of code, go crazy with it
	public UnityEngine.UI.Image BaseImageComponent = null;
	public UnityEngine.UI.Image WartAddOnComponent = null;
	public UnityEngine.UI.Image HornAddOnComponent = null;
	public UnityEngine.UI.Image SmokeImage = null;
	public Sprite HappyPatientSprite = null;
	public Sprite SadPatientSprite = null;
    public Sprite GreenFacedPatientSprite = null;
    public Sprite FrogPatientSprite = null;
    public Sprite HornAddOnSprite = null;
	public Sprite WartAddOnSprite = null;

	public Animator animator = null;
	private string animInBool = "In";

	private PatientSymptomManager Psm;

	private void CheckAssets()
	{
		if( !(BaseImageComponent&&
			  WartAddOnComponent&&
			  HornAddOnComponent&&
			  HappyPatientSprite&&
			  SadPatientSprite&&
			  GreenFacedPatientSprite&&
              FrogPatientSprite&&
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
		ShowSmoke(false);
	}

	// Start is called before the first frame update
    void Start()
    {
        CheckAssets();

		Psm = GameManager.Instance.patientSymptomManager;
		Psm.OnNewPatient += OnNewPatientHander;
		Psm.OnSymptomsChanged += OnSymptomsChangedHandler;
		Init();
    }

	private void OnDestroy()
	{
		if(Psm != null)
		{
			Psm.OnNewPatient -= OnNewPatientHander;
			Psm.OnSymptomsChanged -= OnSymptomsChangedHandler;
		}
	}

	private void OnNewPatientHander(HashSet<eSymptom> symptoms)
	{
		string msg = "new patient: ";
		foreach(eSymptom symp in symptoms)
		{
			msg += $"{symp.ToString()}, ";
		}
		Debug.Log(msg);

		SwitchSprite(symptoms);
	}
	private void OnSymptomsChangedHandler(HashSet<eSymptom> symptoms)
	{
		string msg = "new patient: ";
		foreach(eSymptom symp in symptoms)
		{
			msg += $"{symp.ToString()}, ";
		}
		Debug.Log(msg);
		
		SwitchSprite(symptoms);
	}

	public void ShowSmoke(bool shouldShowSmoke)
    {
		SmokeImage.enabled = shouldShowSmoke;
    }

	public void AnimateOut()
    {
		animator.SetBool(animInBool, false);
    }

	public void AnimateIn()
    {
		animator.SetBool(animInBool, true);
	}

	private void SwitchSprite(HashSet<eSymptom> symptoms)
	{
        WartAddOnComponent.enabled = false;
        HornAddOnComponent.enabled = false;

        if (symptoms.Count == 0)
        {
            BaseImageComponent.sprite = HappyPatientSprite;
        } 
        else
        {
            BaseImageComponent.sprite = SadPatientSprite;
            bool isFrog = false;

            foreach (eSymptom symp in symptoms)
            {
                if (isFrog)
                {
                    break;
                }

                switch (symp)
                {
                    case eSymptom.GreenFace:
                        BaseImageComponent.sprite = GreenFacedPatientSprite;
                        break;

                    case eSymptom.Horn:
                        HornAddOnComponent.enabled = true;
                        break;

                    case eSymptom.Wart:
                        WartAddOnComponent.enabled = true;
                        break;

                    case eSymptom.Frog:
                        BaseImageComponent.sprite = FrogPatientSprite;
                        WartAddOnComponent.enabled = false;
                        HornAddOnComponent.enabled = false;
                        isFrog = true;
                        break;
                }
            }
        }
	}
}
