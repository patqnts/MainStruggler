using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackTime = 0.40f;
    
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator animator;
    [SerializeField] private Joystick joystick;
    public CombatManager combatManager;
    public Transform slashPos;

    public Animator cameraAnimator;

    public float dashSpeed;
    public float dashLength = .5f, dashCooldown = 1f;
    private float activeMoveSpeed;
    private float dashCounter;
    private float dashCoolCounter;

    private Vector2 movement;
    private bool isAttacking;
    private Vector2 lastDirection = Vector2.zero;
    private InventoryManager item;

    public HoldButton holdButton;

    public CharacterGhost ghost;
    private bool canTool = true;
    public Transform dropPos;
    public Collider2D collider;
    public bool isDead = false;

    public float _health;
    public UIHealth uiHealth;
    public float maxHealth;
    public InventoryItem selectedItem;
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
        InventoryManager.instance.ChangeSelectedSlot(0);
        

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
        GameObject cameraHolder = GameObject.Find("cameraHolder");
        if (cameraHolder != null)
        {
            cameraAnimator = cameraHolder.GetComponent<Animator>();
        }
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

        


        Item handle = InventoryManager.instance.GetSelectedItem(false);

        // Handle attack logic
        if (handle != null && handle.type == ItemType.Tool)
        {
            if (Input.GetKey(KeyCode.Space) && !isAttacking && canTool||
                holdButton.isHolding && !isAttacking && canTool)
            {
                canTool = false;
                StartCoroutine(Attack());
                isAttacking = true;
                
            }
        }
        
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && handle != null && handle.type != ItemType.Tool && !isAttacking ||
                Input.GetKeyDown(KeyCode.Space) && handle == null && !isAttacking)
            {
                StartCoroutine(Attack());
                isAttacking = true;

                Item canEat = InventoryManager.instance.GetSelectedItem(item);

                if (canEat != null && canEat.struggler && InventoryManager.instance.GetSelectedItem(item) != null && _health < maxHealth)
                {
                    Debug.Log("StrugglerHeal");

                    uiHealth.StrugglerHeal();
                    InventoryManager.instance.GetSelectedItem(true);
                }
                else if (canEat != null && canEat.consumable && InventoryManager.instance.GetSelectedItem(item) != null && _health < maxHealth)
                {
                    _health++;
                    InventoryManager.instance.GetSelectedItem(true);


                }
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



    public bool holding = false;
    public void isHolding()
    {
        holding =  true;
    }

    private IEnumerator Attack()
    {
        Item slash = InventoryManager.instance.GetSelectedItem(false);
        

        if (slash != null && slash.type == ItemType.Weapon)
        {
            GameObject slashHolder = slash.slashTrailPrefab;

            if (slashHolder != null)
            {
                Vector3 slashPosition = slashPos.position;
                Quaternion slashRotation = Quaternion.identity;
                bool flipSpriteX = false;
                bool flipSpriteY = false;
                //MOVEMENT
                if (movement.x > 0 || lastDirection.x > 0) // Moving right
                {
                    if(lastDirection.y > 0)
                    {
                        slashPosition.x += 0.5f;
                        flipSpriteX = true;
                        slashRotation = Quaternion.Euler(0f, 0f, 50f);
                        //slashRotation = Quaternion.Euler(0f, 0f, -50f); flipx lower right
                      
                    }
                    else if (lastDirection.y < 0)
                    {
                        slashPosition.y -= .6f;
                        slashRotation = Quaternion.Euler(0f, 0f, -50f);
                        flipSpriteX = true;
                        slashPosition.x += 0.3f;
                       
                    }
                    else
                    {
                        slashPosition.x += 0.5f;
                        flipSpriteX = true;
                       
                    }
                    
                }


                else if (lastDirection.x < 0) // Moving left
                {
                    if(lastDirection.y > 0)
                    {
                        slashPosition.x -= 0.5f;
                        slashRotation = Quaternion.Euler(0f, 0f, -50f);

                    }
                    else if(lastDirection.y < 0)
                    {
                        slashPosition.y -= .6f;
                        slashPosition.x -= 0.3f;
                        slashRotation = Quaternion.Euler(0f, 0f, -160f);
                        flipSpriteX = true;

                    }
                    else
                    {
                        slashPosition.x -= 0.5f;
                       
                    }

                    
                   
                }
                else if (movement.y > 0 || lastDirection.y > 0) // Moving up
                {

                    slashRotation = Quaternion.Euler(0f, 0f, -110f);
                    flipSpriteY = true;
                }
                else if (movement.y < 0|| lastDirection.y < 0) // Moving down
                {
                    slashPosition.y -= .6f;
                    slashRotation = Quaternion.Euler(0f, 0f, -110f);
                    flipSpriteX = true;
                    
                }
                
               
                // Instantiate the slash prefab
                GameObject slashObject = Instantiate(slashHolder, slashPosition, slashRotation);


                
                // Adjust the sprite renderer flips based on the movement direction
                SpriteRenderer slashSpriteRenderer = slashObject.GetComponent<SpriteRenderer>();
                if (slashSpriteRenderer != null)
                {
                    slashSpriteRenderer.flipX = flipSpriteX;
                    slashSpriteRenderer.flipY = flipSpriteY;
                }



                Rigidbody2D slashRigidbody = slashObject.GetComponent<Rigidbody2D>();
                if (slashRigidbody != null && slash.element == Element.Light)   //LIGHT ELEMENT EFFECT
                {
                    Debug.Log("Light");
                    // Apply forward movement to the slash prefab
                    float moveSpeed = 5f; // Adjust the movement speed as needed
                    Vector2 moveDirection = lastDirection.normalized;
                    slashRigidbody.velocity = moveDirection * moveSpeed;
                }
               


                Destroy(slashObject, .5f);
            }
            else
            {
            
                Debug.Log("No Slash Prefab");
            }
            if (slash.element == Element.Wind)
            {
                ghost.makeGhost = true;
            }
        }
        else
        {
         
            Debug.Log("No weapon for slash");
        }
        
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        
        // Wait for attack time
        yield return new WaitForSeconds(attackTime);
        canTool = true;
       
        // Reset attacking flag and animation
        ghost.makeGhost =false;
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }


    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement.normalized * activeMoveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Perform a lunge while attacking with a non-tool item
            if (isAttacking && InventoryManager.instance.GetSelectedItem(false)?.type == ItemType.Weapon)
            {
                Vector2 lungeDirection = lastDirection.normalized;
               Item weapon = InventoryManager.instance.GetSelectedItem(false);

                float lungeDistance = weapon.lungeDistance; // Adjust the distance as desired
                rb.MovePosition(rb.position + lungeDirection * lungeDistance * Time.fixedDeltaTime);
            }
        }
    }



    public void OnButtonPress()
    {
        Item handle = InventoryManager.instance.GetSelectedItem(false);
        if (!isAttacking && handle != null && handle.type != ItemType.Tool ||
           handle == null && !isAttacking)
        {
            StartCoroutine(Attack());

            Item canEat = InventoryManager.instance.GetSelectedItem(item);

            if (canEat != null && canEat.struggler && InventoryManager.instance.GetSelectedItem(item) != null && _health < maxHealth)
            {
                Debug.Log("StrugglerHeal");

                uiHealth.StrugglerHeal();
                InventoryManager.instance.GetSelectedItem(true);
            }
            else if (canEat != null && canEat.consumable && InventoryManager.instance.GetSelectedItem(item) != null && _health < maxHealth)
            {
                _health++;
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

    

    public void OnHit(float damage, Vector2 knockback)
    {
        // Take damage and apply knockback force
        Health -= damage;
        rb.AddForce(knockback);
        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger("Slight");
        }

        // Play hurt animation
        animator.SetTrigger("Hurt");
        Debug.Log("Current health: " + _health);
        uiHealth.UpdateHealth(_health, maxHealth);
        //rb.drag = 10;

    }

    public void OnHit(float damage)
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger("Slight");
        }
        animator.SetTrigger("Hurt");
        // Take damage
        Health -= damage;
        uiHealth.UpdateHealth(_health, maxHealth);
    }

    public void OnBurn(float damage, float time)
    {
        StartCoroutine(ApplyBurnDamage(damage, time));
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            yield return new WaitForSeconds(1f);

            OnHit(damage);

            elapsedTime += 1f;
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