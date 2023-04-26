using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    public Animator animator;
    public GameObject[] dropPrefab;
    public bool isOpen = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        int randInt = Random.RandomRange(2, 3);

        
    }

    // Update is called once per frame
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Open");
            DropItem();
            isOpen = true;
        }
    }

    private void DropItem()
    {
        if (dropPrefab != null && isOpen != true)
        {
            for(int i= 0; i<=1; i++)
            {
                Instantiate(dropPrefab[Random.Range(0, dropPrefab.Length)], transform.position, Quaternion.identity);
            }

            

        }
    }


}
