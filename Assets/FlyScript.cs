using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyScript : MonoBehaviour, IDamageable
{
    Rigidbody2D rb;
    public float damage = 1f;
    public DetectionZone detectionZone;
    public Collider2D hitCollider;
    public float knockbackForce = 200f;
    public Animator animator;
    private SlimeSpawner spawner;
    public bool isFacingRight = true;
    private NPCManager npcManager;
    public GameObject alarm;
    public float moveSpeed = 500f;
    public float attackRange = 0.5f;
    public GameObject[] dropPrefab;
    [SerializeField] private EnemyHealthBar enemyHealthBar;

    public GameObject enemyHealthObject;
    public GameObject floatingDamage;
    public bool isElemental;
    public SlimeHorde horde;
    public FlyTotem flyHorde;
    public bool isHorde = false;
    public bool isHorde2 = false;
    private CircleCollider2D detectionCollider;
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

                if (isElemental)
                {
                    Debug.Log("elemental monster dies -1");
                    npcManager.OnElementalDestroyed();
                }
                else
                {
                    Debug.Log("Normal monster dies -1");
                    npcManager.OnEnemyDestroyed();
                }

                if (isHorde)
                {
                    Debug.Log("IS HORDE");
                    horde.PrefabDestroyed();
                }
                if (isHorde2)
                {
                    Debug.Log("IS HORDE2");
                    flyHorde.PrefabDestroyed();
                }

                // DropItem();
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject, 1.2f);
                }

                InventoryManager.instance.ReduceDurability();
            }
        }
        get
        {
            return _health;
        }
    }

    private float _health;
    public float maxHealth = 10;

    // Projectile Shooting Variables
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileCooldown = 2f;
    private float lastProjectileTime;
    public float shootingDistance = 3f;

    private void Awake()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawner = FindObjectOfType<SlimeSpawner>();
        npcManager = FindObjectOfType<NPCManager>();
        horde = FindObjectOfType<SlimeHorde>();
        flyHorde = FindObjectOfType<FlyTotem>();
        if (isHorde || isHorde2)
        {
            detectionCollider = detectionZone.GetComponent<CircleCollider2D>();
            detectionCollider.radius = 8f;
        }
        _health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (detectionZone.detectedObj.Count > 0)
        {
            enemyHealthObject.SetActive(true);
            alarm.gameObject.SetActive(true);
            Invoke("DeactivateAlarm", 1.2f);

            // Check the distance to the player
            float distanceToPlayer = Vector2.Distance(transform.position, detectionZone.detectedObj[0].transform.position);

            if (distanceToPlayer > shootingDistance)
            {
                // Move towards the player if the distance is greater than the shooting distance
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
            else if (Time.time - lastProjectileTime >= projectileCooldown)
            {
                // Shoot a projectile if enough time has passed since the last shot
                animator.SetTrigger("Shoot");
                ShootProjectile();
                lastProjectileTime = Time.time;
            }
        }
        else
        {
            enemyHealthObject.SetActive(false);
        }
    }


    void DeactivateAlarm()
    {
        alarm.gameObject.SetActive(false);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        enemyHealthBar.UpdateHealthBar(_health, maxHealth);

        // Create a new GameObject with the floating damage value
        var floatingDamageGO = Instantiate(floatingDamage, transform.position, Quaternion.identity);
        floatingDamageGO.GetComponent<TextMesh>().text = damage.ToString();

        // Destroy the floating damage after a set amount of time
        Destroy(floatingDamageGO, 1f);

        rb.AddForce(knockback);
        animator.SetTrigger("Hurt");
        Debug.Log(Health);

        if (_health <= 0)
        {
            enemyHealthObject.SetActive(false);
            Destroy(floatingDamageGO, 1f);
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        enemyHealthBar.UpdateHealthBar(_health, maxHealth);

        // Create a new GameObject with the floating damage value
        var floatingDamageGO = Instantiate(floatingDamage, transform.position, Quaternion.identity);
        floatingDamageGO.GetComponent<TextMesh>().text = damage.ToString();

        if (_health <= 0)
        {
            enemyHealthObject.SetActive(false);
            Destroy(floatingDamageGO, 1f);
        }
        else
        {
            // Destroy the floating damage after a set amount of time
            Destroy(floatingDamageGO, 1f);
        }

        animator.SetTrigger("Hurt");
        Debug.Log(Health);
    }

    private bool isBurning = false;
    public void OnBurn(float damage, float time)
    {
        if (!isBurning)
        {
            StartCoroutine(ApplyBurnDamage(damage, time));
        }

        Debug.Log("BURRRRN");
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time && _health > 0)
        {
            isBurning = true;
            yield return new WaitForSeconds(1f);

            OnHit(damage);
            Debug.Log(isBurning);

            elapsedTime += 1f;
        }

        isBurning = false;
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

    private void ShootProjectile()
    {
        Vector2 direction = (detectionZone.detectedObj[0].transform.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction * projectileSpeed;
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
