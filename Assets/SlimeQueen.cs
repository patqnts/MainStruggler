using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeQueen : MonoBehaviour, IDamageable
{
    public float _health,maxHealth;
    private float currentHealth;
    public DetectionZone playerDetectionZone;
    public GameObject slimeMinionPrefab;
  
    
    public float projectileCooldown = 3f;
    public float stunDuration = 3f;
    public float projectileSpeed = 5f;
    public GameObject projectilePrefab;
    public float projectileDamage = 5f;
    public float projectileKnockbackForce = 200f;
    public float pushbackForce = 1000f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isStunned = false;
    public Collider2D collider;
    private float projectileTimer = 0f;
    private List<GameObject> slimeMinions = new List<GameObject>();
    public GameObject[] dropPrefab;

    public AudioSource[] slimeQueenAudios;

    [SerializeField] private EnemyHealthBar healthBar;
    public GameObject enemyHealthObject;
    public GameObject SlimeHealthUI;
    public bool isDead = false;
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                isDead = true;
                Cellular cellular = FindObjectOfType<Cellular>();
                cellular.isDeadSlime = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                collider.enabled = false;
                //enemyHealthObject.SetActive(false);
                for(int x=0; x<15; x++)
                {
                    GameObject projectile = Instantiate(slimeMinionPrefab, transform.position, Quaternion.identity);
                }
                
                Animator slimeHealthAnimator = SlimeHealthUI.GetComponent<Animator>();
                if (slimeHealthAnimator != null)
                {
                    slimeHealthAnimator.SetTrigger("Disappear");
                }
                //DropItem();
                animator.SetTrigger("Death");
                Destroy(gameObject, 3.3f);

                
            }
        }
        get
        {
            return _health;
        }
    }
    public void Boing()
    {
        slimeQueenAudios[0].Play();
    }
    public void Hit()
    {
        slimeQueenAudios[1].Play();
    }
    public void Death()
    {
        slimeQueenAudios[2].Play();
    }
    private void DropItem()
    {
        if (dropPrefab != null)
        {
            foreach (var prefab in dropPrefab)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }
    }
    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = _health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            // Boss is dead, do something here
            Destroy(gameObject);
        }

        if (!isStunned)
        {
            // Check if player is nearby
            if (playerDetectionZone.detectedObj.Count > 0)
            {
                // Summon slime minions if not at maximum capacity and cooldown has passed

                
                // Shoot projectiles if cooldown has passed
                if (projectileTimer <= 0)
                {
                    ShootProjectiles();
                    ShootSlime();
                    projectileTimer = projectileCooldown;
                }

                // Move away from player
                Vector2 direction = (transform.position - playerDetectionZone.detectedObj[0].transform.position).normalized;
                rb.AddForce(direction * pushbackForce * Time.deltaTime);

                // Check if the boss is stuck and unable to move
                if (rb.velocity.magnitude < 0.1f && projectileTimer > 0)
                {
                    // Boss is stuck, move in a random direction away from the player
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    rb.AddForce(randomDirection * pushbackForce * Time.deltaTime);
                }

            }
            
            
            
        }
        else
        {
            // Boss is stunned, countdown stun timer
            Stun(stunDuration);
            stunDuration -= Time.deltaTime;
            if (stunDuration <= 0)
            {
                // Stun is over, reset variables
                isStunned = false;
                animator.SetBool("Stunned", false);
                
                projectileTimer = 0f;
            }
        }

        // Countdown timers
     

        projectileTimer -= Time.deltaTime;
    }




    private void ShootSlime()
    {
        // Shoot projectiles at the player
        if (playerDetectionZone.detectedObj.Count > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 direction = (playerDetectionZone.detectedObj[0].transform.position - transform.position).normalized;
                GameObject projectile = Instantiate(slimeMinionPrefab, transform.position, Quaternion.identity);

                Rigidbody2D projectileRigidbody = projectile.GetComponentInChildren<Rigidbody2D>();
                projectileRigidbody.velocity = direction * projectileSpeed;

                // Convert the direction to Vector3 before adding it to the position
                Vector3 projectileOffset = new Vector3(direction.x, direction.y, 0f);
                projectile.transform.position += projectileOffset;

                projectileRigidbody.velocity = direction * projectileSpeed;
            }
        }
    }

    private void ShootProjectiles()
    {
        // Shoot projectiles at the player
        if (playerDetectionZone.detectedObj.Count > 0)
        {
            Vector2 direction = (playerDetectionZone.detectedObj[0].transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            Destroy(projectile, 2f); // Destroy projectile after 5 seconds if it doesn't hit anything
        }
    }




    public void Stun(float duration)
    {
        isStunned = true;
        animator.SetBool("Stunned", true);
        stunDuration = duration;
    }

    public void Pushback(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
        healthBar.UpdateHealthBar(_health, maxHealth);
        animator.SetTrigger("Hurt");
        Debug.Log(Health);
    }

    public void OnHit(float damage)
    {
        healthBar.UpdateHealthBar(_health, maxHealth);
        if (!isDead)
        {
            Health -= damage;
        }
       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Push the player back
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(pushDirection * (pushbackForce*4));
        }
    }
    public void OnBurn(float damage, float time)
    {
        if(Health > 0 && !isDead)
        {
            StartCoroutine(ApplyBurnDamage(damage, time));
        }
        
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time && Health > 0)
        {
            yield return new WaitForSeconds(1f);

            OnHit(damage);

            elapsedTime += 1f;
        }
    }
    public void OnDark(float time)
    {
        StartCoroutine(Slow(time));

    }

    public IEnumerator Slow(float time)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(time);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}