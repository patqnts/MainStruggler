using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float damage = 1;
    public float knockbackForce;
    private Rigidbody2D rb;
    public Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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


                // Apply knockback force to the player
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (collision.transform.position - transform.position).normalized;
                    damageable.OnHit(damage);
                    rb.AddForce(direction * knockbackForce);
                }
            }
            animator.SetTrigger("Drop");
            // Destroy the projectile
            Destroy(gameObject,0.5f);
        }
    }
}