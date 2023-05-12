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

   

    public float dashSpeed;
    public float dashLength = .5f, dashCooldown = 1f;
    private float activeMoveSpeed;
    private float dashCounter;
    private float dashCoolCounter;

    private Vector2 movement;
    private bool isAttacking;
    private Vector2 lastDirection = Vector2.zero;
    private InventoryManager item;

    public CharacterGhost ghost;

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
        activeMoveSpeed = moveSpeed;


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
        //  movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        // Set animator parameters for movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Dash logic
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isAttacking && movement != Vector2.zero && dashCounter <= 0)
        {

            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                ghost.makeGhost = true;
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;


                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ignore"), true);
            }


            Debug.Log("DASH");
        }

        if (dashCounter > 0)
        {

            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                ghost.makeGhost = false;
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;

                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ignore"), false);
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }





        // Handle attack logic
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
            isAttacking = true;
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
            rb.MovePosition(rb.position + movement.normalized * activeMoveSpeed * Time.fixedDeltaTime);
            
        }

        

    }

    public void OnButtonPress()
    {
        if (!isAttacking)
        {
            StartCoroutine(Attack());

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
    }
    public void Dash()
    {
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            ghost.makeGhost = true;
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;


            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ignore"), true);
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