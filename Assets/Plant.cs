using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IDamageable
{

    public Rigidbody2D rb;
    private float damage = 0f;
    public float explosionDamage = 5f;
    public DetectionZone detectionZone;
    public Collider2D hitCollider;
    private float knocks = 0f;
    public float knockbackForce = 100f;
    public Animator animator;
    public float moveSpeed = 5f;
    public float _health = 3f;

    public bool isFacingRight = true;
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                //rb.constraints = RigidbodyConstraints2D.FreezeAll;
                moveSpeed = 0;
                hasExploded = true;
                hitCollider.enabled = false;
                animator.SetTrigger("Explode");
                timer = 3f;
                Destroy(gameObject, 2f);


            }
        }
        get
        {
            return _health;
        }
    }

    private bool hasDetectedPlayer = false;
    private bool hasExploded = false;
    public bool isDetecting = false; // New variable
    private float explosionTimer = 3f;
    private float timer = 0f;
    public GameObject explosionEffect;
    public Transform explosionPoint;
    public float explosionRadius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionZone = GetComponent<DetectionZone>();
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
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDetecting)
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
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Enemy"))
        {
            damageableObject.OnHit(damage, knockback);
           
        }

    }





}
