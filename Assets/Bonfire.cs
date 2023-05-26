using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private float damage = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
        if  (collision.gameObject != null && damageableObject != null)
        {
            if (collision.gameObject.CompareTag("Object"))
            {
                damageableObject.OnHit(damage);
            }
        }
        
    }
}
