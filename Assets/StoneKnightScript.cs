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
    public GameObject[] dropPrefab;
    public float attackRange = 1.5f;
    [SerializeField] private EnemyHealthBar healthBar;
    public GameObject enemyHealthObject;
    public float _health,maxHealth = 10f;
    private NPCManager npcManager;
    public bool isElemental;

    public AudioSource[] stone;
    public void PlayWake()
    {
        stone[0].Play();
    }
    public void DeathSound()
    {
        stone[1].Play();
    }
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                moveSpeed = 0;
                hitCollider.enabled = false;
                enemyHealthObject.SetActive(false);
                DeathSound();
                animator.SetTrigger("Death");
                InventoryManager.instance.ReduceDurability();
                //DropItem();
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
                
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject, 1.2f);
                }
                
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
            PlayWake();
            enemyHealthObject.SetActive(true);
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
           
        }
        else
        {
            isDetecting = false;
            
            animator.SetBool("Detect", false);
            enemyHealthObject.SetActive(false);
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
       
        healthBar.UpdateHealthBar(_health, maxHealth);
        animator.SetTrigger("Hurt");
        if(_health <= 0)
        {

            enemyHealthObject.SetActive(false);
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;


        healthBar.UpdateHealthBar(_health, maxHealth);
        animator.SetTrigger("Hurt");
        if (_health <= 0)
        {
            enemyHealthObject.SetActive(false);
        }
    }
    private Coroutine burnCoroutine;

    public void OnBurn(float damage, float time)
    {
        if (burnCoroutine == null)
        {
            burnCoroutine = StartCoroutine(ApplyBurnDamage(damage, time));
            Debug.Log("BURRRRN");
        }
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time && _health > 0)
        {
            yield return new WaitForSeconds(1f);

            OnHit(damage);
            Debug.Log("BURRRRN");

            elapsedTime += 1f;
        }

        burnCoroutine = null;
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
