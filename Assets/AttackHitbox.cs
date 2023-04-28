using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Collider2D hitCollider;
    public float knockbackForce = 5000f;

    public float damage = 1f;



    public InventoryManager inventoryManager;
    private void Update()
    {
        Item item = InventoryManager.instance.GetSelectedItem(false);
    }
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        // Get the Collider2D component of the hitbox
        hitCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();

        if (hitCollider != null)
        {
            Vector3 parentPos = transform.parent.position;
            Vector2 direction = (collision.gameObject.transform.position - parentPos).normalized;
            Vector2 knockback = direction * knockbackForce;
            if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Tree")|| collision.gameObject.CompareTag("Rock"))
            {
                Item selectedItem = inventoryManager.GetSelectedItem(false);
                damage = selectedItem != null ? selectedItem.weaponDamage : 1f;               
                damageableObject.OnHit(damage, knockback);

                

            }
        }
        else
        {
            Debug.Log("Wall");
        }
    }
}
