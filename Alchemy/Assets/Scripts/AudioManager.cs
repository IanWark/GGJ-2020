using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } = null;

    [SerializeField]
    private AudioSource soundSource = null;

    [SerializeField]
    private AudioSource musicSource = null;

    [Header("Clips")]
    [SerializeField]
    public AudioClip musicClip = null;

    [SerializeField]
    public AudioClip openBookClip = null;

    [SerializeField]
    public AudioClip pageTurnClip = null;

    [SerializeField]
    public AudioClip brewClip = null;

    [SerializeField]
    public AudioClip slotIngredientClip = null;

	[SerializeField]
    public AudioClip newPatientClip = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void PlayOpenBook()
    {
        PlaySound(openBookClip);
    }

    public void PlayPageTurn()
    {
        PlaySound(pageTurnClip);
    }

    public void PlayBrew()
    {
        PlaySound(brewClip);
    }

    public void PlaySlotIngredientClip()
    {
        PlaySound(slotIngredientClip);
    }

	public void PlayNewPatientClip()
    {
        PlaySound(newPatientClip);
    }
}
