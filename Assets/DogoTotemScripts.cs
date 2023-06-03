using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogoTotemScripts : MonoBehaviour
{
    public Animator animator;
    public GameObject Dogo;
    public GameObject DogoUI;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DogoUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DogoUI.SetActive(false);
        }
    }

    IEnumerator InstantiateDogo()
    {
        yield return new WaitForSeconds(4.8f);
        Instantiate(Dogo, transform.position, Quaternion.identity);
    }

    public void DogoStart()
    {
        int natureCost = 1;
        int stoneCost = 1;
        int slimeCost = 1;

        if(InventoryManager.instance.GetItemCount("Key of Nature") < 1||
            InventoryManager.instance.GetItemCount("Key of Stone") < 1||
            InventoryManager.instance.GetItemCount("Key of Slime") < 1)
        {
            Debug.Log("Keys are not complete yet.");
            return;
        }
        else
        {
            animator.SetTrigger("Unlock");
            Destroy(gameObject, 5f);
            StartCoroutine(InstantiateDogo());

            InventoryManager.instance.RemoveItem("Key of Slime",1);
            InventoryManager.instance.RemoveItem("Key of Stone", 1);
            InventoryManager.instance.RemoveItem("Key of Nature", 1);
            DogoUI.SetActive(false);
        }
    }
}
