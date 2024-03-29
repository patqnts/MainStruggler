using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDamage : MonoBehaviour
{
    public Collider2D hitCollider;
    public float knockbackForce = 5000f;

    public float damage = 25f;


    public bool isPlayer = false;
    private string objectTag = "";
   
    void Start()
    {
        
        // Get the Collider2D component of the hitbox
        hitCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();

        if (hitCollider != null && collision.gameObject != null && damageableObject != null)
        {
            Vector3 parentPos = transform.position;
            Vector2 direction = (collision.gameObject.transform.position - parentPos).normalized;
            Vector2 knockback = direction * knockbackForce;
            if (isPlayer)
            {
                objectTag = "Enemy";
            }
            else
            {
                objectTag = "Player";
            }

            if (collision.gameObject.CompareTag(objectTag))
             {
                damageableObject.OnHit(damage, knockback);
             }

                
        }
        

    }



}
