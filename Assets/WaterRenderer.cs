using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRenderer : MonoBehaviour
{
    public Transform playerTransform;  // Reference to the player's transform.
    public float renderDistance = 5f; // The distance at which to render the water.

    private Renderer waterRenderer;

    void Start()
    {
        // Get the Renderer component of the water GameObject.
        waterRenderer = GetComponent<Renderer>();

        // Find the player GameObject using its tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            //
        }
    }

    void Update()
    {
        if (playerTransform == null || waterRenderer == null)
        {
            
            return;
        }

        // Calculate the distance between the player and the water GameObject.
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Check if the distance is greater than the renderDistance.
        if (distance > renderDistance)
        {
            // Disable the renderer.
            waterRenderer.enabled = false;
        }
        else
        {
            // Enable the renderer.
            waterRenderer.enabled = true;
        }
    }
}
