using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource[] tree;
    
    public void TreeHit()
    {
        tree[0].Play();
    }

    public void TreeDestroyed()
    {
        tree[1].Play();
    }
}
