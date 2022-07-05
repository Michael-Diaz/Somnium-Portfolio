using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private AudioSource SFX;
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] alerts;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SFX = GetComponent<AudioSource>();
        // playFootstep();
    }

    public void playFootstep(int footstepCounter)
    {
        SFX.PlayOneShot(footsteps[footstepCounter % 4]);
    }

    public void playAlertClose() 
    {
        SFX.PlayOneShot(alerts[0]);
    }

    public void playAlertFar() 
    {
        SFX.PlayOneShot(alerts[1]);
    }
}
