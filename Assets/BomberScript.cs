using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour, IDamageable
{
    public float moveSpeed = 400f;
    public Animator animator;
    public Collider2D hitCollider;
    Rigidbody2D rb;
    public DetectionZone detectionZone;
   
    private Vector2 movement;

    public bool Agro = false;

    public GameObject plantBombPrefab;  // Reference to the plant bomb prefab
    public Transform summonPoint;       // Transform indicating the summon point in front of the enemy base

    public float summonCooldown = 5f;    // Cooldown between summoning plant bombs
    private float summonTimer = 0f;


    [SerializeField] private EnemyHealthBar healthBar;
    public GameObject enemyHealthObject;
    public float _health, maxHealth = 10f;
    private NPCManager npcManager;
    public float throwCooldown = 2f;
    public GameObject[] dropPrefab;
    public GameObject SlimeHealthUI;
    public float Timer;
    public Animator cameraAnimator;

    private float followDistance = 2.5f;
    public float jumpRange = 7f;
    public bool isDead = false;


    public AudioSource[] bombersounds;
    public float Health


    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                
                hitCollider.enabled = false;
                isDead = true;
                Cellular cellular = FindObjectOfType<Cellular>();
                cellular.isDeadBomber = true;
                moveSpeed = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                hitCollider.enabled = false;
                //enemyHealthObject.SetActive(false);
                Animator slimeHealthAnimator = SlimeHealthUI.GetComponent<Animator>();
                if (slimeHealthAnimator != null)
                {
                    slimeHealthAnimator.SetTrigger("Disappear");
                }


                    animator.SetTrigger("Death");
                npcManager.OnEnemyDestroyed();
                Destroy(gameObject, 3.2f);
            }
        }
        get
        {
            return _health;
        }
    }
    public void DropItem() // inserted to death animation
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

    private bool isJumpAttacking = false;

    public void DeathSound()
    {
        bombersounds[4].Play();
    }
    private void Update()
    {
        if (detectionZone.detectedObj.Count > 0 )
        {
            //Noticegameobject .true
            if(Agro == true)
            {
                // Get the position of the closest detected object (player)
                Vector2 playerPos = detectionZone.detectedObj[0].transform.position;

                if (Timer <= 0 && !isJumpAttacking)
                {
                    // Check if the player is within jump range
                    if (Vector2.Distance(playerPos, transform.position) <= jumpRange)
                    {

                        StartCoroutine(PerformJumpAttack(playerPos));
                    }
                }
                if (summonTimer <= 0f && !isJumpAttacking)
                {
                    StartCoroutine(SummonPlantBomb(playerPos));
                    // Summon a plant bomb


                    // Reset the summon timer
                    summonTimer = summonCooldown;
                }

                if (playerPos != null)
                {
                    // Calculate the direction towards the player
                    Vector2 directionToPlayer = playerPos - (Vector2)transform.position;

                    // Check if the enemy is too close to the player
                    float distanceToPlayer = directionToPlayer.magnitude;

                    if (distanceToPlayer <= followDistance && !isJumpAttacking)
                    {
                        // Calculate the target position that maintains a certain distance from the player
                        Vector2 targetPosition = playerPos - directionToPlayer.normalized * followDistance;

                        // Calculate the movement towards the target position
                        movement = (targetPosition - (Vector2)transform.position).normalized;
                        animator.SetBool("Run", true);
                        // Apply the movement force
                        rb.AddForce(movement * moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // Move directly towards the player if the distance is greater than the follow distance
                        movement = directionToPlayer.normalized;
                        animator.SetBool("Run", true);
                        // Apply the movement force
                        rb.AddForce(movement * moveSpeed * Time.deltaTime);
                    }
                }
            }
            
        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("Run", false);
        }
        
        summonTimer -= Time.deltaTime;
        Timer -= Time.deltaTime;
        
    }
    public bool isStomping = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
        if (collision.CompareTag("Player") && collision.gameObject != null)
        {
            if (isStomping)
            {
                damageableObject.OnHit(1f);
                damageableObject.OnDark(1.5f);
            }
            
        }
    }
    public void Stomp()
    {
        isStomping = true;
    }
    public void StompSound()
    {
        bombersounds[1].Play();
    }
    public void StepSound()
    {
        bombersounds[2].Play();
    }
    public void HitSound()
    {
        bombersounds[3].Play();
    }
    public void StompEnd()
    {
        isStomping = false;
    }

    private IEnumerator SummonPlantBomb(Vector2 playerPos)
    {
        bombersounds[0].Play();
        animator.SetTrigger("Summon");
        moveSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        float angleIncrement = 120f; // Angle between each bomb plant in the triangle formation
        float distanceFromPlayer = 1f; // Distance of each bomb plant from the player
        moveSpeed = 400f;
        for (int i = 0; i < 3; i++)
        {
            // Calculate the position of the current bomb plant based on the player's position
            float angle = i * angleIncrement;
            Vector2 offset = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
            Vector2 bombPos = playerPos + (offset * distanceFromPlayer);

            // Instantiate the plant bomb at the calculated position
            GameObject plantBomb = Instantiate(plantBombPrefab, bombPos, Quaternion.identity);
            // Set the parent of the plant bomb to the enemy's parent (e.g., enemy base)
            plantBomb.transform.SetParent(transform.parent);
        }
    }
    private IEnumerator PerformJumpAttack(Vector2 playerPos)
    {
        // Start the jump attack
        isJumpAttacking = true;
        moveSpeed = 0;
        // Store the initial position before the jump
        Vector2 initialPosition = transform.position;

        // Trigger the jump animation
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(1f);
        
        // Calculate the direction towards the player
        Vector2 directionToPlayer = playerPos - (Vector2)transform.position;
        directionToPlayer.Normalize();

        // Set the jump duration
        float jumpDuration = .46f;
        float elapsedDuration = 0f;
        
        while (elapsedDuration < jumpDuration)
        {
            // Calculate the target position based on the player's position
            Vector2 targetPosition = playerPos;

            // Move towards the target position
            transform.position = Vector2.Lerp(initialPosition, targetPosition, elapsedDuration / jumpDuration);

            elapsedDuration += Time.deltaTime;
            yield return null;
        }
        
        // Reset the enemy's position to the player's position
        transform.position = playerPos;

        isJumpAttacking = false;
        moveSpeed = 400f;
        // Reset the jump attack timer
        Timer = throwCooldown;
        Debug.Log("Reset");
    }

    
    public void OnHit(float damage, Vector2 knockback)
    {
        Agro = true;
        HitSound();
        Item weapon = InventoryManager.instance.GetSelectedItem(false);
        if (weapon != null && weapon.type != ItemType.Weapon && !isDead)
        {
            float reducedDamage = damage * 0.05f;

            Health -= reducedDamage;
            rb.AddForce(knockback);
            Debug.Log("Reduced");
        }
        else
        {
            float icreaseDamage = damage * 1f;

            Health -= icreaseDamage;
            rb.AddForce(knockback);
            Debug.Log("Increased");
        }


        healthBar.UpdateHealthBar(_health, maxHealth);
        animator.SetTrigger("Hurt");

    }

    public void OnHit(float damage)
    {
        Agro = true;
        HitSound();
        healthBar.UpdateHealthBar(_health, maxHealth);
        if (!isDead)
        {
            Health -= damage;
        }
       
    }
    public float treeDamage = 5000f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Rock"))
        {
            damageable.OnHit(treeDamage);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Agro = true;
        }
    }

    public void walkShake()
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger("Slight");
        }
    }

    private bool isBurning = false;
    public void OnBurn(float damage, float time)
    {
        if (!isBurning && !isDead)
        {
            Debug.Log("BURRRRN");
            StartCoroutine(ApplyBurnDamage(damage, time));
        }

       
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
        throw new System.NotImplementedException();
    }
}
