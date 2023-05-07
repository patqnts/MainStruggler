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
    private SlimeSpawner spawner;
    public bool isFacingRight = true;

    public GameObject alarm;
    public float moveSpeed = 500f;
    public float attackRange = 0.5f;
    public GameObject[] dropPrefab;
    
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                moveSpeed = 0;

                hitCollider.enabled = false;
                animator.SetTrigger("Death");
                DropItem();
                Destroy(gameObject,1.2f);
               
               
                
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
        spawner = FindObjectOfType<SlimeSpawner>();
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
            // Get the position of the closest detected object
            Vector2 playerPos = detectionZone.detectedObj[0].transform.position;

            // Check if the player is close enough to attack
            float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
            if (distanceToPlayer <= attackRange)
            {
                animator.SetTrigger("Attack");
            }
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
        if (collision.gameObject.CompareTag("Player"))
        {
            
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
