using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WanderScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource footsteps;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private Vector2 decisionTime = new Vector2(1f, 4f);
    

    private float decisionTimeCount;
    private Vector2 currentDirection;
    public GameObject notice;

    // The maximum distance to check for obstacles in front of the character
    public float obstacleCheckDistance = 1.0f;

    // The layer mask to use for obstacle detection
    public LayerMask obstacleLayerMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
        currentDirection = ChooseMoveDirection();
    }
    public void FootSteps()
    {
        footsteps.Play();
    }
    private void Update()
    {
        if (decisionTimeCount > 0f)
        {
            decisionTimeCount -= Time.deltaTime;
        }
        else
        {
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
            currentDirection = ChooseMoveDirection();
        }

        animator.SetFloat("Horizontal", currentDirection.x);
        animator.SetFloat("Vertical", currentDirection.y);
        animator.SetFloat("Speed", currentDirection.sqrMagnitude);

        // If the notice object is active, stop moving
        if (notice.activeSelf)
        {
            currentDirection = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
    }

    private void FixedUpdate()
    {
        Vector2 movePosition = rb.position + currentDirection.normalized * moveSpeed * Time.fixedDeltaTime;

        // Check for obstacles in front of the character
        RaycastHit2D obstacleHit = Physics2D.Raycast(rb.position, currentDirection, obstacleCheckDistance, obstacleLayerMask);
        if (obstacleHit.collider != null)
        {
            // Choose a new direction
            currentDirection = ChooseMoveDirection();
            return;
        }

        rb.MovePosition(movePosition);
    }

    private Vector2 ChooseMoveDirection()
    {
        Vector2[] directions = new Vector2[] { new Vector2(-1, 1).normalized, new Vector2(1, 1).normalized, new Vector2(-1, -1).normalized, new Vector2(1, -1).normalized };
        Vector2 newDirection = directions[Random.Range(0, directions.Length)];

        // Check if the new direction collides with any obstacles
        RaycastHit2D obstacleHit = Physics2D.Raycast(rb.position, newDirection, obstacleCheckDistance, obstacleLayerMask);
        int i = 0;
        while (i < directions.Length - 1 && obstacleHit.collider != null)
        {
            i++;
            newDirection = directions[i];
            obstacleHit = Physics2D.Raycast(rb.position, newDirection, obstacleCheckDistance, obstacleLayerMask);
        }

        return newDirection;
    }

}
