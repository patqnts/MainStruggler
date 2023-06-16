using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource blacksmith;
    public AudioSource[] hut;

    public AudioSource[] SmithInteractionSounds;
    public void PlayAudioSource()
    {

        blacksmith.Play();
       
    }

    public void Hut()
    {
        hut[Random.Range(0,3)].Play();
    }
}
