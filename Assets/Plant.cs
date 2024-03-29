using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IDamageable
{
   
    public Rigidbody2D rb;
    private float damage = 1f;
    public float explosionDamage = 5f;
    public DetectionZone detectionZone;
    public Collider2D hitCollider;
    private float knocks = 0f;
    public float knockbackForce = 100f;
    public Animator animator;
    public float moveSpeed = 5f;
    public float _health = 3f;
    private NPCManager npcManager;
    public bool isFacingRight = true;
    public GameObject[] dropPrefab;
    public bool isElemental;
    public bool isFire = false;
    public bool isDark = false;

    public AudioSource[] audio;
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                //rb.constraints = RigidbodyConstraints2D.FreezeAll;
                moveSpeed = 0;
                
                hitCollider.enabled = false;
                animator.SetTrigger("Explode");
                timer = 3f;
                
                if (isElemental)
                {
                    Debug.Log("Elemental monster dies -1");
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

    public void Jump()
    {
        audio[0].Play();
    }
    public void Clock()
    {
        audio[1].Play();
    }
    public void Explode()
    {
        audio[2].Play();
    }
    private void DropItem()
    {
        if (dropPrefab != null)
        {

            Instantiate(dropPrefab[Random.Range(0, dropPrefab.Length)], transform.position, Quaternion.identity);

        }
    }
    private bool hasDetectedPlayer = false;
    private bool hasExploded = false;
    public bool isDetecting = false; // New variable
    public float explosionTimer = 3f;
    private float timer = 0f;
   
    public Transform explosionPoint;
    public float explosionRadius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionZone = GetComponent<DetectionZone>();
        npcManager = FindObjectOfType<NPCManager>();
    }

    public void ExplosionDamageUpdate()
    {
        damage = explosionDamage;
        knocks = knockbackForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDetectedPlayer && !hasExploded)
        {
            timer += Time.deltaTime;
            animator.SetTrigger("Run");
            
            if (timer >= explosionTimer)
            {
                hasExploded = true;

                // Disable hit collider and stop movement
                hitCollider.enabled = false;
                moveSpeed = 0f;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                // Change detection zone radius
                DropItem();

                // Play explosion animation
                animator.SetTrigger("Explode");

                // Disable collider and destroy object after animation finishes
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0))
                {
                   
                    hitCollider.enabled = false;
                    
                    Destroy(gameObject, 1f);
                }
            }
        }
    }



    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
    }

    public void OnHit(float damage)
    {
        Health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            isDetecting = true; // Set isDetecting to true when player enters trigger
            // hitCollider.enabled = false;
            animator.SetTrigger("Detect");
            hasDetectedPlayer = true;
            Clock();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDetecting && !hasExploded)
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed;
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();



        // Calculate knockback based on facing direction
        Vector2 direction = transform.right * (isFacingRight ? 1 : -1);
        Vector2 knockback = direction * knocks;
        knockback.y = Random.Range(-1f, 1f) * knocks;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isDark)
            {
                damageableObject.OnDark(1f);
            }
            else if (isFire)
            {
                damageableObject.OnBurn(1, 2);
            }
            damageableObject.OnHit(explosionDamage, knockback);
           
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
        isBurning = true;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            yield return new WaitForSeconds(1f);

            OnHit(damage);
            Debug.Log("BURRRRN");

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
