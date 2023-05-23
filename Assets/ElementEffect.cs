using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementEffect : MonoBehaviour
{
    public Collider2D hitCollider;
    public float damageAmount = 2f; // Amount of damage to inflict
    public float knockbackForce = 500f;
    void Start()
    {
        hitCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
      
        Vector2 direction = (collision.gameObject.transform.position - transform.position).normalized;
        Vector2 knockback = direction * knockbackForce;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            

            damageableObject.OnHit(damageAmount, knockback);
        }

    }
}
