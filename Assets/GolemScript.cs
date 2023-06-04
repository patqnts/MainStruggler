using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemScript : MonoBehaviour, IDamageable
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
    public float throwRange = 10f; // Adjust this value for the desired throw range
    [SerializeField] private EnemyHealthBar healthBar;
    public GameObject enemyHealthObject;
    public float _health, maxHealth = 10f;
    private NPCManager npcManager;
    public GameObject[] dropPrefab;
    public GameObject stonePrefab;
    public Transform throwPoint;
    public float throwForce = 500f;
    public float throwCooldown = 2f;
    private float throwTimer = 0f;


    public Animator cameraAnimator;
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                Cellular cellular = FindObjectOfType<Cellular>();
                cellular.isDeadGolem = true;
                moveSpeed = 0;
                hitCollider.enabled = false;
                DropItem();
                //enemyHealthObject.SetActive(false);
                animator.SetTrigger("Death");
                npcManager.OnEnemyDestroyed();
                Destroy(gameObject, 1.2f);
            }
        }
        get
        {
            return _health;
        }
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

    private void Start()
    {
        detectionZone = GetComponentInChildren<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        npcManager = FindObjectOfType<NPCManager>();


        GameObject cameraHolder = GameObject.Find("cameraHolder"); // Assuming the cameraHolder is in the scene
        if (cameraHolder != null)
        {
            cameraAnimator = cameraHolder.GetComponent<Animator>();
        }
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
                if (!isAttacking && detectionZone.detectedObj[0].transform.position != null)
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

            if (distanceToPlayer <= throwRange && !isAttacking && Timer <= 0 && throwTimer <= 0)
            {
                // Throw a stone at the player
                StartCoroutine(ThrowStone(playerPos));
                throwTimer = throwCooldown;
            }

            // Update throw timer
            if (throwTimer > 0)
            {
                throwTimer -= Time.deltaTime;
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

    private IEnumerator ThrowStone(Vector2 playerPos)
    {
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(0.33f); // Wait for the attack animation to reach the throw frame

        // Instantiate the stone projectile
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D stoneRb = stone.GetComponent<Rigidbody2D>();

        // Calculate the direction towards the player's position
        Vector2 directionToPlayer = (playerPos - (Vector2)throwPoint.position).normalized;

        // Calculate the maximum distance the stone should travel
        float maxDistance = Vector2.Distance(throwPoint.position, playerPos);

        // Set the initial velocity of the stone to move towards the player
        stoneRb.velocity = directionToPlayer * throwForce;

        // Wait until the stone reaches the maximum distance
        yield return new WaitUntil(() => stone != null && Vector2.Distance(stone.transform.position, throwPoint.position) >= maxDistance);

        // Check if the stone is still valid before stopping its movement
        if (stone != null)
        {
            // Stop the stone's movement
            stoneRb.velocity = Vector2.zero;

            // Trigger animation for the stone dropping
            stone.GetComponent<Animator>().SetTrigger("Drop");
            if (cameraAnimator != null)
            {
                cameraAnimator.SetTrigger("Shake");
            }
            // Destroy the stone after a certain time
            Destroy(stone, 5f);

        }
    }

    public void walkShake()
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger("Slight");
        }
    }


    public float coolDown = 0.25f;
    public float Timer = 0f;

    private IEnumerator Attack()
    {
        // Set attacking flag and play animation
        isAttacking = true;

        animator.SetBool("isAttacking", true);
        Timer = coolDown;

        // Wait for attack time
        yield return new WaitForSeconds(0.45f);

        // Reset attacking flag and animation
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Item weapon = InventoryManager.instance.GetSelectedItem(false);
        if(weapon != null && weapon.type != ItemType.Tool )
        {
            float reducedDamage = damage * 0.05f;

            Health -= reducedDamage;
            rb.AddForce(knockback);
            Debug.Log("Reduced");
        }
        else
        {
            float icreaseDamage = damage * 3.5f;

            Health -= icreaseDamage;
           // rb.AddForce(knockback);
            Debug.Log("Increased");
        }
        

        healthBar.UpdateHealthBar(_health, maxHealth);
        animator.SetTrigger("Hurt");
      
    }

    public void OnHit(float damage)
    {
        healthBar.UpdateHealthBar(_health, maxHealth);
        Health -= damage;
    }
    public float treeDamage = 5000f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Rock"))
        {
            damageable.OnHit(treeDamage);
        }
    }
    private bool isBurning = false;
    public void OnBurn(float damage, float time)
    {
        if (!isBurning && _health > 0)
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
