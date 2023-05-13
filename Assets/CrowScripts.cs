using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowScripts : MonoBehaviour
{
    public Animator animator;
    public DetectionZone detectionZone;
    public bool isLookingRight = false;
    public bool isLookingLeft = false;
    private Movement player;

    private float attackTimer = 0f;
    private float attackInterval = 2f;
    private int numAttacks = 0;


    public GameObject notice;
    public GameObject NoticeUI;
    public bool isAttacking = false;
    private void Start()
    {
        player = FindObjectOfType<Movement>();
    }
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
            NoticeUI.SetActive(true);
            notice.SetActive(true);



            if (isAttacking)
            {
                // Reduce the player's health every attackInterval seconds
                animator.SetBool("Attack", true);
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    attackTimer = 0f;
                    player.OnHit(1);

                    numAttacks++;

                    if (numAttacks >= 2)
                    {
                        StopAttack();
                        numAttacks = 0;
                        Debug.Log("Success");
                        
                    }
                }

                // Make the player immovable while attacking
                 Debug.Log("NOT now!");
            }
            else
            {
               StopAttack();
               animator.SetBool("Attack", false);
               animator.SetBool("isLookingRight", false);
               animator.SetBool("isLookingLeft", false);
               Debug.Log("Can Move now!");
                
            }
        }
        else if (detectionZone.detectedObj.Count == 0)
        {
            numAttacks = 0;
            // Reset the animation state when the player is not detected
            animator.SetBool("isLookingRight", false);
            animator.SetBool("isLookingLeft", false);
            animator.SetBool("Attack", false);
            NoticeUI.SetActive(false);
            notice.SetActive(false);
            isAttacking = false;
        }

    }


        
    public void Attack()
    {

        isAttacking = true;
        
    }

    public void StopAttack()
    {
        isAttacking = false;
    }
    
    
}
