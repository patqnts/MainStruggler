using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public Collider2D hitCollider;
    public float knockbackForce = 5000f;

    public float damage = 25f;



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
            if (collision.gameObject.CompareTag("Enemy"))
            {
                damageableObject.OnHit(damage, knockback);
            }


        }


    }
}
