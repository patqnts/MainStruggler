using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderDistance : MonoBehaviour
{
    private Transform myLocation;
    public float renderDistance = 5f;
    

    void Start()
    {
        myLocation = transform; // Set myLocation to the current GameObject's transform
       
    }

    void Update()
    {
        GameObject[] treeObjects = GameObject.FindGameObjectsWithTag("Tree");
        

        foreach (GameObject tree in treeObjects)
        {
            // Calculate the distance between the current waterTile and your location
            float distance = Vector3.Distance(tree.transform.position, myLocation.position);

            // Check if the distance is less than or equal to the renderDistance
            if (distance <= renderDistance)
            {
                // Enable the renderer of the waterTile
                Renderer renderer = tree.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
            else
            {
                // Disable the renderer of the waterTile
                Renderer renderer = tree.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }

        
    }
}
