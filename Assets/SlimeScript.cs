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
    public FlyTotem flyhord;
    public bool isHorde = false;
    public bool isHorde2 = false;
    private CircleCollider2D detectionCollider;
    public AudioSource[] audio;
    public bool isDead = false;
    public void DeathSound()
    {
        audio[0].Play();
    }
    public void BoingSound()
    {
        audio[1].Play();
    }
    public void HurtSound()
    {
        audio[2].Play();
    }
    public void AttackSound()
    {
        audio[3].Play();
    }

    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                moveSpeed = 0;
                isDead = true;
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
                    flyhord.PrefabDestroyed();
                }

                // DropItem();
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject, 1.2f);
                    
                }

                DropItem();
                InventoryManager.instance.ReduceDurability();


            }
        }
        get
        {
            return _health;
        }
    }
    private void Awake()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    // Start is called before the first frame update
    public float _health,maxHealth = 10;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawner = FindObjectOfType<SlimeSpawner>();
        npcManager = FindObjectOfType<NPCManager>();
        horde = FindObjectOfType<SlimeHorde>();
        flyhord = FindObjectOfType<FlyTotem>();
        if (isHorde || isHorde2)
        {
            detectionCollider = detectionZone.GetComponent<CircleCollider2D>();
            detectionCollider.radius = 8f;
        }
        
    }



    private bool canAttack = true;
    private float attackCooldown = 2.0f; // Cooldown duration in seconds
    private float timeSinceLastAttack = 0.0f;

    private void FixedUpdate()
    {
        if (detectionZone.detectedObj.Count > 0)
        {
            enemyHealthObject.SetActive(true);
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

            // Check if the player is close enough to attack and cooldown is over
            float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
            if (distanceToPlayer <= attackRange && canAttack)
            {
                // Perform the attack
                animator.SetTrigger("Attack");

                // Apply cooldown
                canAttack = false;
                timeSinceLastAttack = 0.0f;
            }
        }
        else
        {
            enemyHealthObject.SetActive(false);
        }

        // Update the attack cooldown
        if (!canAttack)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= attackCooldown)
            {
                canAttack = true;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null & collision.gameObject.CompareTag("Player"))
        {
            if (canAttack)
            {
                // Perform the attack
                animator.SetTrigger("Attack");

                // Apply cooldown
                canAttack = false;
                timeSinceLastAttack = 0.0f;
            }
        }
    }
    void DeactivateAlarm()
    {
        alarm.gameObject.SetActive(false);
    }
    public void OnHit(float damage, Vector2 knockback)
    {
        if (!isDead)
        {
            Health -= damage;
        }
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
        if (!isDead)
        {
            Health -= damage;
        }
        enemyHealthBar.UpdateHealthBar(_health, maxHealth);

        // Create a new GameObject with the floating damage value
        var floatingDamageGO = Instantiate(floatingDamage, transform.position, Quaternion.identity);
        floatingDamageGO.GetComponent<TextMesh>().text = damage.ToString();

        // Destroy the floating damage after a set amount of time
        Destroy(floatingDamageGO, 1f);

        
        animator.SetTrigger("Hurt");
        Debug.Log(Health);
        if (Health <= 0)
        {
            enemyHealthObject.SetActive(false);
            Destroy(floatingDamageGO, 1f);
        }

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

        while (elapsedTime < time && Health > 0)
        {
            isBurning = true;
            yield return new WaitForSeconds(1f);

            OnHit(damage);
            Debug.Log(isBurning);

            elapsedTime += 1f;
        }
        isBurning = false;
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
