using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private float moveSpeed;
    [SerializeField] public int durability;
    public ItemNotificationScript itemNotificationScript;

    // Start is called before the first frame update
    public Item item;
    public int itemCount; // New variable to store the item count

    private void Start()
    {
        itemNotificationScript = FindObjectOfType<ItemNotificationScript>();
    }
    public void Initialize(Item item, int count, int durability) 
    {
        this.item = item;
        this.itemCount = count;
        this.durability = durability; 
        sr.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Item checkItem = InventoryManager.instance.GetItemByName(item.name);
           

            bool canAdd = InventoryManager.instance.AddItem(item, durability, itemCount); 
            if (canAdd)
            {             
                
                if (checkItem == null)
                {
                    itemNotificationScript.Notify(item);
                   
                }

                StartCoroutine(MoveAndCollect(other.transform));               
            }
        }
    }


    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(collider);
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0;     
            Destroy(gameObject,0.5f);
        }    
    }  
}
