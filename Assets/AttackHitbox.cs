using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Collider2D hitCollider;
    public float knockbackForce = 5000f;

    public float damage = 1f;

    public Animator cameraAnimator;

    public InventoryManager inventoryManager;
    private void Update()
    {
        Item item = InventoryManager.instance.GetSelectedItem(false);
    }
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        GameObject cameraHolder = GameObject.Find("cameraHolder"); // Assuming the cameraHolder is in the scene
        if (cameraHolder != null)
        {
            cameraAnimator = cameraHolder.GetComponent<Animator>();
        }
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
                if (cameraAnimator != null)
                {
                    cameraAnimator.SetTrigger("Slight");
                }
                if (selectedItem != null && selectedItem.type == ItemType.Weapon)
                {
                    // WEAPON TO ENEMY
                    GameObject elementEffect = selectedItem.elementEffect;
                    if (elementEffect != null)
                    {
                        // Check if there is already an effect attached to the enemy
                        ElementEffect existingEffect = collision.gameObject.GetComponentInChildren<ElementEffect>();

                        if (existingEffect == null)
                        {
                            // No existing effect, instantiate a new one
                            GameObject effect = Instantiate(elementEffect, collision.transform);

                            // Set the position and scale of the effect
                            float yOffset = 0.3f; // Modify this value as needed
                            Vector3 newPosition = collision.transform.position;
                            newPosition.y += yOffset;
                            effect.transform.position = newPosition;
                            effect.transform.localScale = collision.transform.localScale;

                            if (selectedItem.element == Element.Flame) ///ELEMENT MODIFIER
                            {
                                damageableObject.OnBurn(10, 4);
                            }
                            if(selectedItem.element == Element.Dark)
                            {
                                Debug.Log("DARKNESS");
                                damageableObject.OnDark(1f);
                            }

                            Destroy(effect, 4f);
                        }
                    }
                    damage = selectedItem.weaponDamage;
                }
                else if (selectedItem != null && selectedItem.type == ItemType.Tool)
                {
                    //TOOL TO ENEMY
                    damage = selectedItem.weaponDamage * .05f;
                }
                else
                {
                    // Hand only
                    damage = 1f;
                }
            }
            else if (collision.gameObject.CompareTag("Tree"))
            {
                if (selectedItem != null && selectedItem.type == ItemType.Tool)
                {
                    // TOOL TO TREES AND ROCK
                    damage = selectedItem.weaponDamage;
                }
                else if (selectedItem != null && selectedItem.type == ItemType.Weapon)
                {
                    // WEAPON TO TREES AND ROCK
                    damage = selectedItem.weaponDamage * .05f;
                    GameObject elementEffect = selectedItem.elementEffect;
                    if (elementEffect != null)
                    {
                        // Check if there is already an effect attached to the enemy
                        ElementEffect existingEffect = collision.gameObject.GetComponentInChildren<ElementEffect>();

                        if (existingEffect == null)
                        {
                            // No existing effect, instantiate a new one
                            GameObject effect = Instantiate(elementEffect, collision.transform);

                            // Set the position and scale of the effect
                            float yOffset = 0.3f; // Modify this value as needed
                            Vector3 newPosition = collision.transform.position;
                            newPosition.y += yOffset;
                            effect.transform.position = newPosition;
                            effect.transform.localScale = collision.transform.localScale;

                            if (selectedItem.element == Element.Flame)
                            {
                                damageableObject.OnBurn(10, 4);
                            }

                            Destroy(effect, 4f);
                        }
                    }
                }
                else
                {
                    // Hand only
                    damage = 1f;
                } 
            }
            else if (collision.gameObject.CompareTag("Rock"))
            {
                if (selectedItem != null && selectedItem.type == ItemType.Tool)
                {
                    // TOOL TO TREES AND ROCK
                    damage = selectedItem.weaponDamage;
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
           
        }
        
    }



}
