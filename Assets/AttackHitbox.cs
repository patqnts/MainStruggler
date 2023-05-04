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

        if (hitCollider != null && collision.gameObject != null && damageableObject != null)
        {
            Vector3 parentPos = transform.parent.position;
            Vector2 direction = (collision.gameObject.transform.position - parentPos).normalized;
            Vector2 knockback = direction * knockbackForce;

            Item selectedItem = inventoryManager.GetSelectedItem(false);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (selectedItem != null && selectedItem.type == ItemType.Weapon)
                {
                    // WEAPON TO ENEMY
                    damage = selectedItem.weaponDamage;
                }
                else if (selectedItem != null && selectedItem.type == ItemType.Tool)
                {
                    //TOOL TO ENEMY
                    damage = selectedItem.weaponDamage * .6f;
                }
                else
                {
                    // Hand only
                    damage = 1f;
                }
            }
            else if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Rock"))
            {
                if (selectedItem != null && selectedItem.type == ItemType.Tool)
                {
                    // TOOL TO TREES AND ROCK
                    damage = selectedItem.weaponDamage;
                }
                else if (selectedItem != null && selectedItem.type == ItemType.Weapon)
                {
                    // WEAPON TO TREES AND ROCK
                    damage = selectedItem.weaponDamage * .6f;
                }
                else
                {
                    // Hand only
                    damage = 1f;
                } 
            }


            damageableObject.OnHit(damage, knockback);
        }
        else
        {
            Debug.Log("None");
        }
        
    }



}
