using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;
    public float knockbackForce;
    public Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            // Deal damage to the player
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.OnHit(damage);
                animator.SetTrigger("Hit");
                // Apply knockback force to the player
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (collision.transform.position - transform.position).normalized;

                    rb.AddForce(direction * knockbackForce);
                }
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}