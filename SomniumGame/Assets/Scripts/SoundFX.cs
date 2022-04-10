using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private AudioSource SFX;
    [SerializeField] private AudioClip[] footsteps;
    
    
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
}
