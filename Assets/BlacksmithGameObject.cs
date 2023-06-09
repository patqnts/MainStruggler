using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource blacksmith;
    public void PlayAudioSource()
    {

        blacksmith.Play();
       
    }
}
