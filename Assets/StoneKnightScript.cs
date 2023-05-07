using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneKnightScript : MonoBehaviour, IDamageable
{
    public float moveSpeed = 400f;
    public Animator animator;
    public Collider2D hitCollider;
    Rigidbody2D rb;
    public DetectionZone detectionZone;
    private bool isAttacking;
    private Vector2 movement;
  
    private Vector2 lastDirection = Vector2.zero;
    public bool isDetecting;

    public float attackRange = 1.5f;

    public float _health = 10f;
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
                Destroy(gameObject, 1.2f);
            }
        }
        get
        {
            return _health;
        }
    }
    private void Start()
    {
        detectionZone = GetComponentInChildren<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }

        animator.SetFloat("LastHorizontal", lastDirection.x);
        animator.SetFloat("LastVertical", lastDirection.y);

        if (detectionZone.detectedObj.Count > 0)
        {
            animator.SetTrigger("Wake");

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") || animator.GetCurrentAnimatorStateInfo(0).IsName("Still"))
            {
                movement = Vector2.zero;
            }
            else
            {
                animator.SetBool("Detect", true);
                isDetecting = true;
                if (!isAttacking)
                {
                    movement = (detectionZone.detectedObj[0].transform.position - transform.position).normalized;
                    rb.AddForce(movement * moveSpeed * Time.deltaTime);
                }

            }
            // Get the position of the closest detected object
            Vector2 playerPos = detectionZone.detectedObj[0].transform.position;

            // Check if the player is close enough to attack
            float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
            if (distanceToPlayer <= attackRange && !isAttacking && Timer <= 0)
            {
                // Attack the player
                StartCoroutine(Attack());
                
            }
           
        }
        else
        {
            isDetecting = false;
            movement = Vector2.zero;
            animator.SetBool("Detect", false);
        }
        Timer -= Time.deltaTime;
    }

    public float coolDown = 0.25f;
    public float Timer = 0f;
    private IEnumerator Attack()
    {
        // Set attacking flag and play animation
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        Timer = coolDown;
        //hel


        // Wait for attack time
        yield return new WaitForSeconds(0.45f);

        // Reset attacking flag and animation
        isAttacking = false;
        animator.SetBool("isAttacking", false);



    }





    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
       
        animator.SetTrigger("Hurt");
    }

    public void OnHit(float damage)
    {
        Health -= damage;
    }
    
}
