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
    private float desiredDistance = 3f;
    public float _health = 10;
    public bool isAttacking = false;
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

    public float projectileCooldown;  // the cooldown time between projectile throws
    public int projectilesToThrow;  // the number of projectiles to throw each time

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPoss = player.transform.position;
        Vector2 enemyPos = transform.position;
        float distance = Vector2.Distance(playerPoss, enemyPos);
        if (player != null && distance <5f)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 playerPos = player.transform.position;
            Vector2 direction = (playerPos - (Vector2)transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * 250);
            Destroy(projectile, 4f);
        }
    }

    private void FixedUpdate()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerPos = player.transform.position;
            Vector2 enemyPos = transform.position;
            float distance = Vector2.Distance(playerPos, enemyPos);

            if (distance > 3f)
            {
                // move towards the player
                Vector2 direction = (playerPos - enemyPos).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
            }
            else if (distance < 3f)
            {
                // move away from the player
                Vector2 direction = (enemyPos - playerPos).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
            }
        }

        if (!isAttacking)
        {
            moveSpeed = 500f;
        }
        else
        {
            moveSpeed = 0;
        }
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
}
