using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinSavePoint : MonoBehaviour
{
    // Static list to keep track of all the ruins in the game
    private static List<RuinSavePoint> ruins = new List<RuinSavePoint>();

    // Keep track of the last ruin the player entered
    private static RuinSavePoint lastRuin = null;

    // Boolean flag to indicate if the player has entered the ruin
    public bool playerEntered = false;

    // Called when the object is first created
    private void Awake()
    {
        // Add this ruin to the list of ruins
        ruins.Add(this);
    }

    // Called when the object is destroyed
    private void OnDestroy()
    {
        // Remove this ruin from the list of ruins
        ruins.Remove(this);
    }

    // Called when a collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the collider is the player, set the flag to true and update the last ruin
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
            lastRuin = this;
            Debug.Log("SAVEPOINT");
        }
        else if (other.CompareTag("Tree") || other.CompareTag("Rock"))
        {
            Destroy(other.gameObject);
            Debug.Log("Tree Destoryed");
        }
        else if (other.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRigidbody = other.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemyRigidbody.AddForce(knockbackDirection * 10f, ForceMode2D.Impulse);
            }
        }
    }

    // Called when a collider exits this trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // If the collider is the player, set the flag to false
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }

    // Called when the player dies
    public static void PlayerDied()
    {
        // If a last ruin has been set, teleport the player to it
        if (lastRuin != null)
        {
            Movement playerController = FindObjectOfType<Movement>();
            playerController.transform.position = lastRuin.transform.position;
        }
        else
        {
            // If no last ruin has been set, teleport the player to the first ruin in the list
            if (ruins.Count > 0)
            {
                Movement playerController = FindObjectOfType<Movement>();
                playerController.transform.position = ruins[0].transform.position;
            }
        }
    }
}
