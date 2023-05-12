using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowScripts : MonoBehaviour
{
    public Animator animator;
    public DetectionZone detectionZone;
    public bool isLookingRight = false;
    public bool isLookingLeft = false;

    void Update()
    {
        if (detectionZone.detectedObj.Count > 0)
        {
            // Get the direction of the player relative to the NPC
            Vector2 direction = detectionZone.detectedObj[0].transform.position - transform.position;

            if (direction.x > 0)
            {
               
                    // Set the "isLookingRight" parameter to true to play the animation
                animator.SetBool("isLookingRight", true);
                animator.SetBool("isLookingLeft", false);


            }
            else if (direction.x < 0)
            {
                // The player is to the left of the NPC
               
                    // Set the "isLookingLeft" parameter to true to play the animation
                    animator.SetBool("isLookingLeft", true);
                animator.SetBool("isLookingRight", false);


            }
        }
        else if (detectionZone.detectedObj.Count == 0)
        {
            // Reset the animation state when the player is not detected
            animator.SetBool("isLookingRight", false);
            animator.SetBool("isLookingLeft", false);
            
        }
    }
}
