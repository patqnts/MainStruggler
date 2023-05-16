using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Slash()
    {
        animator.SetTrigger("Slash");
        Debug.Log("Slash");
    }
}
