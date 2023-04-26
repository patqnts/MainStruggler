using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    public Item item;


    public void Initialize(Item item)
    {
        this.item = item;
        sr.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {  
                StartCoroutine(MoveAndCollect(other.transform));
          
        }
         
        
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(collider);
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0; 
        }
        InventoryManager.instance.AddItem(item);
        Debug.Log("Item Received:" + item);
        Destroy(gameObject);



            
    }
    
}
