using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource footsteps;
    public AudioSource hit;

    public AudioSource []heals;
    public void FootSteps()
    {
        footsteps.Play();
    }

    public void ReceiveHitSound()
    {
        hit.Play();
    }
   
    public void eatSound()
    {
        heals[0].Play();
        heals[3].Play();
    }
    public void strugglerBottleSound()
    {
        heals[1].Play();
        heals[3].Play();
    }
    public void heartContainerSound()
    {
        heals[2].Play();
    }
    public void dashSound()
    {
        heals[4].Play();
    }


}
