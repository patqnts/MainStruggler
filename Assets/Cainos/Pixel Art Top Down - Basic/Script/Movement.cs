using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackTime = 0.2f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Joystick joystick;


    private Vector2 movement;
    private bool isAttacking;
    private Vector2 lastDirection = Vector2.zero;
    private InventoryManager item;
    public float Health { get; set; }

    private void Awake()
    {
        Health = 100f;
        
    }
    private void Start()
    {
        item = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        // Get input for movement
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
       // movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        // Set animator parameters for movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Handle attack logic
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());


            // Decrement consumable item after attacking
            Item canEat = InventoryManager.instance.GetSelectedItem(item);
            if (canEat != null && canEat.consumable && InventoryManager.instance.GetSelectedItem(item) != null)
            {
                InventoryManager.instance.GetSelectedItem(true);
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
                InventoryManager.instance.GetSelectedItem(true);
            }
        }
    }

    private IEnumerator Attack()
    {
        // Set attacking flag and play animation
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
        rb.drag = 10f;

        // Play hurt animation
        animator.SetTrigger("Hurt");
    }

    public void OnHit(float damage)
    {
        // Take damage
        Health -= damage;
    }
}