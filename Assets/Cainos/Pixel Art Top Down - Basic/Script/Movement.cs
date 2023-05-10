using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackTime = 0.2f;
    [SerializeField] private float eatTime = 1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Joystick joystick;

    private Vector2 movement;
    private bool isAttacking;
    private Vector2 lastDirection = Vector2.zero;
    private InventoryManager item;
    public Transform dropPos;
    
    public Collider2D collider;
    public bool isDead = false;

    public float _health;
    public UIHealth uiHealth;
    public float maxHealth;
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                collider.enabled = false;
                
                animator.SetTrigger("Death");
                Invoke("Dead", 2f);
                // Destroy(gameObject, 2.3f);
            }
            
          
        }
        get
        {
            return _health;
        }
    }
    public void Dead()
    {
        
        isDead = true;
        

    }
    public void DropItemPlayer()
    {
        InventoryManager.instance.DropAllItems(dropPos);
    }
    public void Respawn()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        collider.enabled = true;
        isDead = false;
        _health = maxHealth;
        animator.SetTrigger("Respawn");
    }

    
    private void Start()
    {
        uiHealth = FindObjectOfType<UIHealth>();
        uiHealth.SetMaxHearts(maxHealth);
       
        item = GetComponent<InventoryManager>();
        uiHealth.UpdateHealth(_health, maxHealth);
        
    }

    private void Update()
    {
        
        // Get input for movement
         movement.x = joystick.Horizontal;
         movement.y = joystick.Vertical;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Set animator parameters for movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Handle attack logic
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());

           // 
            
            
            // Decrement consumable item after attacking
            Item canEat = InventoryManager.instance.GetSelectedItem(item);
            if (canEat != null && canEat.consumable && InventoryManager.instance.GetSelectedItem(item) != null && _health < maxHealth)
            {
                _health++;
                InventoryManager.instance.GetSelectedItem(true);

                
                
            }
            else if (canEat != null && InventoryManager.instance.GetSelectedItem(item) != null && canEat.name == "Fruit") //HEART CONTAINER PLACEHOLDER
            {
                
                uiHealth.AddHeart();
                InventoryManager.instance.GetSelectedItem(true);

            }
            else
            {
                return;
            }
        }


        // Update last direction if moving
        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }

        // Set animator parameters for last direction
        animator.SetFloat("LastHorizontal", lastDirection.x);
        animator.SetFloat("LastVertical", lastDirection.y);
        uiHealth.UpdateHealth(_health, maxHealth);
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void OnButtonPress()
    {
        if (!isAttacking)
        {
            StartCoroutine(Attack());

            // Decrement consumable item after attacking
            Item canEat = InventoryManager.instance.GetSelectedItem(item);
            if (canEat != null && canEat.consumable && InventoryManager.instance.GetSelectedItem(item) != null)
            {
                _health++;
                uiHealth.UpdateHealth(_health, maxHealth);
                InventoryManager.instance.GetSelectedItem(true);
                
                Debug.Log("Added Health" + _health);
            }
        }
    }

    private IEnumerator Attack()
    {
        
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        // Wait for attack time
        yield return new WaitForSeconds(attackTime);

        // Reset attacking flag and animation
        isAttacking = false;
        animator.SetBool("isAttacking", false);

        
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        // Take damage and apply knockback force
        Health -= damage;
        rb.AddForce(knockback);
        
       
        // Play hurt animation
        animator.SetTrigger("Hurt");
        Debug.Log("Current health: " + _health);
        uiHealth.UpdateHealth(_health, maxHealth);
        //rb.drag = 10;

    }

    public void OnHit(float damage)
    {
        // Take damage
        Health -= damage;
        uiHealth.UpdateHealth(_health, maxHealth);
    }
}