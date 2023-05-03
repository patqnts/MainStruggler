using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogoTotemScripts : MonoBehaviour
{

    
    public Animator animator;
    // Start is called before the first frame update
    public GameObject Dogo;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Unlock");
            Destroy(gameObject, 5f);
            Instantiate(Dogo, transform.position, Quaternion.identity);
            Invoke("InstantiateDogo", 5f);  // invoke the method after 5 seconds
        }
    }

    private void InstantiateDogo()
    {
        Instantiate(Dogo, transform.position, Quaternion.identity);
    }

}
