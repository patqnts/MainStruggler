using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private float moveSpeed;
    [SerializeField] public int durability;

    // Start is called before the first frame update
    public Item item;
    public int itemCount; // New variable to store the item count


    public void Initialize(Item item, int count, int durability) // Added durability parameter
    {
        this.item = item;
        this.itemCount = count;
        this.durability = durability; // Assign durability value
        sr.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int x = 0; x < itemCount; x++)
            {
                bool canAdd = InventoryManager.instance.AddItem(item, durability); // Pass durability to AddItem
                if (canAdd)
                {
                    StartCoroutine(MoveAndCollect(other.transform));
                }
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
       // InventoryManager.instance.AddItem(item);
        Debug.Log("Item Received:" + item); 
    }
}
