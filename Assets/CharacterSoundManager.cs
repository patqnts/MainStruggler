using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource footsteps;
    public AudioSource hit;
    public void FootSteps()
    {
        footsteps.Play();
    }

    public void ReceiveHitSound()
    {
        hit.Play();
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
