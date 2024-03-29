using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHItBox : MonoBehaviour
{
    public Collider2D hitCollider;
    public float knockbackForce = 5000f;

    public float damage = 1f;
    public bool isFire = false;
    public bool isDark = false;
    // Start is called before the first frame update
    void Start()
    {
        hitCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
        Vector3 parentPos = transform.parent.position;
        Vector2 direction = (collision.gameObject.transform.position - parentPos).normalized;
        Vector2 knockback = direction * knockbackForce;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (isDark)
            {
                damageableObject.OnDark(1f);
            }
            else if (isFire)
            {
                damageableObject.OnBurn(1, 2);
            }
            damageableObject.OnHit(damage, knockback);
        }
            
    }
}
