using UnityEngine;
using System.Collections.Generic;

public class BirdController : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius for detecting the player
    public float movementSpeed = 2f; // Speed at which the bird moves towards a new perch

    private GameObject player; // Reference to the player
    private GameObject currentPerch; // Current perch object
    private Vector3 targetPerch; // Target perch position
    private bool isFlying = false; // Flag to indicate if the bird is flying

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentPerch = gameObject; // Assume the bird is initially perched on itself
        targetPerch = transform.position;
    }

    private void Update()
    {
        if (isFlying)
        {
            // Move the bird towards the target perch
            transform.position = Vector3.MoveTowards(transform.position, targetPerch, movementSpeed * Time.deltaTime);

            // Check if the bird has reached the target perch
            if (transform.position == targetPerch)
            {
                isFlying = false;
                // Set idle or resting animation here
            }
        }
        else if (IsPlayerInRadius())
        {
            // Player is within the detection radius, fly away from the player
            Vector3 direction = transform.position - player.transform.position;
            targetPerch = transform.position + direction.normalized * detectionRadius;
            isFlying = true;
            // Trigger the flying animation here
        }
        else if (!IsPlayerInMap() || !ArePerchesAvailable())
        {
            // Player is out of the map or no perches available, find a new perch on a tree
            if (currentPerch.CompareTag("Tree"))
            {
                // Bird is already on a tree, no need to find a new perch
                return;
            }
            else
            {
                // Bird is without a perch, find a new perch on a tree
                FindNewPerch();
            }
        }
    }



    private bool IsPlayerInRadius()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= detectionRadius;
    }

    private bool IsPlayerInMap()
    {
        // Assuming your map has a boundary defined by minimum and maximum coordinates

        // Get the bounds of your map
        Vector3 mapMinBounds = new Vector3(0f, 0f, 0f); // Minimum X and Y coordinates of the map
        Vector3 mapMaxBounds = new Vector3(90f, 90f, 0f); // Maximum X and Y coordinates of the map

        // Check if the player's position is within the map bounds
        Vector3 playerPosition = player.transform.position;
        if (playerPosition.x >= mapMinBounds.x && playerPosition.x <= mapMaxBounds.x &&
            playerPosition.y >= mapMinBounds.y && playerPosition.y <= mapMaxBounds.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ArePerchesAvailable()
    {
        // Example: Check if there are any perches (objects with the "Tree" tag) in the scene
        GameObject[] perches = GameObject.FindGameObjectsWithTag("Tree");
        if (perches.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (!isFlying)
            {
                // Player entered the detection radius, find a new perch
                FindNewPerch();
            }
        }
    }

    private void FindNewPerch()
    {
        // Find a suitable perch (tree or rock) for the bird
        // You can implement your own logic here based on your scene setup

        // Example: Get all perches with the "Tree" tag
        GameObject[] perches = GameObject.FindGameObjectsWithTag("Tree");

        // If no perches found, exit the method
        if (perches.Length == 0)
            return;

        // Exclude the current perch from the list of available perches
        List<GameObject> availablePerches = new List<GameObject>(perches.Length - 1);
        foreach (GameObject perch in perches)
        {
            if (perch != currentPerch)
            {
                availablePerches.Add(perch);
            }
        }

        // If no available perches found, exit the method
        if (availablePerches.Count == 0)
            return;

        // Find the closest perch to the bird's current position
        GameObject closestPerch = availablePerches[0];
        float closestDistance = Vector3.Distance(transform.position, closestPerch.transform.position);

        for (int i = 1; i < availablePerches.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, availablePerches[i].transform.position);
            if (distance < closestDistance)
            {
                closestPerch = availablePerches[i];
                closestDistance = distance;
            }
        }

        // Set the target perch and start flying towards it
        currentPerch = closestPerch;
        targetPerch = closestPerch.transform.position;
        isFlying = true;

        // Trigger the flying animation here
    }
}
