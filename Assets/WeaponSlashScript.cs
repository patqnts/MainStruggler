using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlashScript : MonoBehaviour
{
    public GameObject slashPrefab; // Prefab for the slash effect
    public Transform slashSpawnPoint; // Transform representing the spawn point of the slash effect
    public float slashDuration = 0.5f; // Duration of the slash effect before it's destroyed

    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    public void Slash()
    {
        // Instantiate the slash effect in all four directions
       
            GameObject slash = Instantiate(slashPrefab, slashSpawnPoint.position, Quaternion.identity);
            
        
    }

   
}
