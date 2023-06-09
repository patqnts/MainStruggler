using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float movementSpeed = 2f; // Speed at which the bird moves towards a new perch
    public float distanceFactor = 1.5f; // Factor to multiply the direction vector for targetPosition calculation

    private GameObject player; // Reference to the player
    private Vector3 targetPosition; // Target position to fly towards
    private bool isFlying = false; // Flag to indicate if the bird is flying
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public AudioSource takeoff;
    public void FlySound()
    {
        takeoff.Play();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFlying)
        {
            // Move the bird towards the target position diagonally
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            // Check if the bird has reached the target position
            if (transform.position == targetPosition)
            {
                isFlying = false;
                animator.SetBool("Fly", false);
                // Set idle or resting animation here
            }
        }

        // Check the movement direction and flip the SpriteRenderer accordingly
        if (transform.position.x > targetPosition.x)
        {
            spriteRenderer.flipX = true; // Bird is moving to the left, flip the sprite
        }
        else if (transform.position.x < targetPosition.x)
        {
            spriteRenderer.flipX = false; // Bird is moving to the right, flip the sprite back
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            // Player entered the trigger collider, make the bird fly away diagonally
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = direction.x; // Restrict the Y component of the direction vector to maintain diagonal movement
            targetPosition = transform.position + direction * 10f * distanceFactor; // Move the bird farther away in the calculated direction
            isFlying = true;
            animator.SetBool("Fly", true);
            FlySound();
            // Trigger the flying animation here
        }
    }

    private void OnBecameInvisible()
    {
        if (isFlying)
        {
            Destroy(gameObject,1.3f);
        }
        
    }
}
