using UnityEngine;

public class RangeSlash : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movementDirection;

    private void Start()
    {
        // Set the initial movement direction based on the negative value of the slash prefab's x position
        movementDirection = new Vector2(-transform.position.x, 0f).normalized;

        // Adjust the movement direction based on the intended behavior for each direction
        AdjustMovementDirection();
    }

    private void Update()
    {
        // Move the range slash based on the movement direction and speed
        Vector2 movement = movementDirection * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void AdjustMovementDirection()
    {
        // Adjust the movement direction based on the intended behavior for each direction
        if (transform.position.x > 0f) // Moving towards the right
        {
            Debug.Log("Right");
            movementDirection = -movementDirection;
        }
        else if (transform.position.x < 0f) // Moving towards the left
        {
            Debug.Log("Left");
            // No adjustment needed since the initial movement direction is already correct
        }
        else if (transform.position.y > 0f) // Moving upwards
        {
            Debug.Log("up");
            // Modify movementDirection as needed for upward movement
            // Example: movementDirection += new Vector2(0f, 1f); // Move upwards with an additional offset
        }
        else if (transform.position.y < 0f) // Moving downwards
        {
            Debug.Log("down");
            // Modify movementDirection as needed for downward movement
            // Example: movementDirection -= new Vector2(0f, 1f); // Move downwards with an additional offset
        }
    }
}
