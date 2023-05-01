using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour, IDamageable
{
    Rigidbody2D rb;
    public float damage = 1f;
    public DetectionZone detectionZone;
    public Collider2D hitCollider;
    public float knockbackForce = 200f;
    public Animator animator;

    public bool isFacingRight = true;

    public GameObject alarm;
    public float moveSpeed = 500f;

    public GameObject[] dropPrefab;
    
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                DropItem();
                Destroy(gameObject);
            }
        }
        get
        {
            return _health;
        }
    }
    

    // Start is called before the first frame update
    public float _health = 10;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    

    private void FixedUpdate()
    {

        if (detectionZone.detectedObj.Count > 0)
        {
            alarm.gameObject.SetActive(true);
            Invoke("DeactivateAlarm", 1.2f);
            Vector2 direction = (detectionZone.detectedObj[0].transform.position - transform.position).normalized;

            // Update facing direction based on movement direction
            if (direction.x > 0)
            {
                isFacingRight = true;
            }
            else if (direction.x < 0)
            {
                isFacingRight = false;
            }
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
        }


    }

    void DeactivateAlarm()
    {
        alarm.gameObject.SetActive(false);
    }
    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
        animator.SetTrigger("Hurt");
        Debug.Log(Health);
    }

    public void OnHit(float damage)
    {
        Health -= damage;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();



        // Calculate knockback based on facing direction
        Vector2 direction = transform.right * (isFacingRight ? 1 : -1);
        Vector2 knockback = direction * knockbackForce;
        knockback.y = Random.Range(-1f, 1f) * knockbackForce;
        if (collision.gameObject.CompareTag("Player"))
        {
            damageableObject.OnHit(damage, knockback);
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetTrigger("Attack");
        }


    }

    private void DropItem()
    {
        if (dropPrefab != null)
        {    
            
                Instantiate(dropPrefab[Random.Range(0,dropPrefab.Length)], transform.position, Quaternion.identity);
                 
        }
    }

}
