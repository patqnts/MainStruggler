using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyAI : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb;
    Vector2 originalVelocity;
    public float damage = 1f;
    public DetectionZone detectionZone;
    public Collider2D hitCollider;
    public Animator animator;
    public GameObject projectilePrefab;
    public float moveSpeed;
    public float pushbackForce = 1000f;
    public float _health = 10;
    public bool isAttacking = false;
    public bool isSecondPhase = false;
    public bool isAlive;
    public bool inTransition = false;

    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                if (isSecondPhase)
                {
                    isAlive = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    hitCollider.enabled = false;
                    animator.SetTrigger("Death");
                    Destroy(gameObject, 4f);
                }
                else
                {
                    // Enter second phase
                    isAlive = false;
                    isSecondPhase = true;

                    inTransition = true;
                    animator.SetTrigger("Phase2");
                    animator.SetBool("SecondPhase", true);

                    _health = 10;
                    isAlive = true;
                    lastChargeDashTime = 5f;

                    //animator.Play("Idle2");
                }
            }
        }
        get
        {
            return _health;
        }
    }

    public float projectileCooldown;  // the cooldown time between projectile throws
    public int projectilesToThrow;  // the number of projectiles to throw each time


    public float chargeDashSpeed;
    public float chargeDashDuration;
    public float chargeDashCooldown;
    private float lastChargeDashTime= 5f;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(ThrowProjectilesWithCooldown());
    }

    private IEnumerator ThrowProjectilesWithCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(projectileCooldown);
            for (int i = 0; i < projectilesToThrow; i++)
            {
                // Stop moving before throwing projectiles


                ThrowProjectileAtPlayer();

                // Resume moving after throwing projectiles

               
                yield return new WaitForSeconds(0.75f);  // wait a bit between each projectile
                isAttacking = false;
            }
        }
    }

   private void ThrowProjectileAtPlayer()
    {
        if (_health <= 0)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Target");
        Vector2 playerPoss = player.transform.position;
        Vector2 enemyPos = transform.position;
        float distance = Vector2.Distance(playerPoss, enemyPos);
        if (player != null && distance <5f && !isSecondPhase)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 playerPos = player.transform.position;
            Vector2 direction = (playerPos - (Vector2)transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * 250);
            Destroy(projectile, 4f);
        }
        else if (player != null && distance < 5f && isSecondPhase && isAlive)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            
            
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 playerPos = player.transform.position;
            Vector2 direction = (playerPos - (Vector2)transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * 600);
            Destroy(projectile, 4f);
        }
    }

    private void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Target");
        if (player != null)
        {
            Vector2 playerPos = player.transform.position;
            Vector2 enemyPos = transform.position;
            float distance = Vector2.Distance(playerPos, enemyPos);

            if (Time.time - lastChargeDashTime > chargeDashCooldown && distance < 8f && isSecondPhase && isAlive)
            {
                // Charge dash towards the player
                StartCoroutine(ChargeDash(playerPos));
                lastChargeDashTime = Time.time;
            }
            else if (distance > 3.5f)
            {
                // move towards the player
                Vector2 direction = (playerPos - enemyPos).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
            }
            else if (distance < 3.5f)
            {
                // move away from the player
                Vector2 direction = (enemyPos - playerPos).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
            }
        }

        if (!isAttacking)
        {
            if (isSecondPhase)
            {
                moveSpeed = 650f;
            }
            else
            {
                moveSpeed = 500f;
            }
            
        }
        else
        {
            moveSpeed = 0;
             
        }
    }

    private IEnumerator ChargeDash(Vector2 targetPos)
    {
        isAttacking = true;
        animator.SetTrigger("Dash");

        // Stop moving before charging
        originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        // Move towards the player at high speed
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        rb.AddForce(direction * chargeDashSpeed);

        yield return new WaitForSeconds(chargeDashDuration);

        // Resume moving after charging
        rb.velocity = originalVelocity;

        // End the attack
        isAttacking = false;
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);

        Debug.Log(Health);
    }

    public void OnHit(float damage)
    {
        Health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();

        if (collision.gameObject.CompareTag("Player"))
        {
            damageableObject.OnHit(damage);
            // Push the player back
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(pushDirection * (pushbackForce * 4));
        }
    }
}
