using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource footsteps;
    public AudioSource woosh;
    public void FootSteps()
    {
        footsteps.Play();
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
