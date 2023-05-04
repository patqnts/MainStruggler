using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogoTotemScripts : MonoBehaviour
{
    public Animator animator;
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
            StartCoroutine(InstantiateDogo());
        }
    }

    IEnumerator InstantiateDogo()
    {
        yield return new WaitForSeconds(4.8f);
        Instantiate(Dogo, transform.position, Quaternion.identity);
    }
}
