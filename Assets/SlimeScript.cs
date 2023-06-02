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
               

                DropItem();
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
    }

    

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

            // Check if the player is close enough to attack
            float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
            if (distanceToPlayer <= attackRange)
            {
                animator.SetTrigger("Attack");
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

        // Destroy the floating damage after a set amount of time
        Destroy(floatingDamageGO, 1f);

        
        animator.SetTrigger("Hurt");
        Debug.Log(Health);
        if (_health <= 0)
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

    private void DropItem()
    {
        if (dropPrefab != null)
        {    
            
                Instantiate(dropPrefab[Random.Range(0,dropPrefab.Length)], transform.position, Quaternion.identity);
                 
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
