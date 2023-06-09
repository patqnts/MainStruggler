using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowScripts : MonoBehaviour
{
    public Animator animator;
    public DetectionZone detectionZone;
    public bool isLookingRight = false;
    public bool isLookingLeft = false;
    public GameObject dashAbility;
    public string inactiveObjectName = "Dash";


    private Movement player;

    private float attackTimer = 0f;
    private float attackInterval = 2f;
    private int numAttacks = 0;

    public bool dashUnlocked;
    public GameObject notice;
    public GameObject NoticeUI;
    public bool isAttacking = false;
    private void Start()
    {
        player = FindObjectOfType<Movement>();
        GameObject canvas = GameObject.Find("Controller");
        dashAbility = canvas.transform.Find(inactiveObjectName).gameObject;
        LoadSystem load = FindObjectOfType<LoadSystem>();
        dashUnlocked = load.dashPassValue;
        if (dashAbility != null)
        {
            if (dashUnlocked)
            {
                dashAbility.SetActive(true);
            }
            else
            {
                dashAbility.SetActive(false);
            }
            
        }

        
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
            
            if(NoticeUI != null)
            {
                NoticeUI.SetActive(true);
                notice.SetActive(true);
            }
           



            if (isAttacking)
            {
                // Reduce the player's health every attackInterval seconds
                animator.SetBool("Attack", true);
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                attackTimer += Time.deltaTime;
                
                if (attackTimer >= attackInterval)
                {
                    attackTimer = 0f;
                    player.OnHit(1);

                    numAttacks++;

                    if (numAttacks >= 10)
                    {
                        StopAttack();
                        numAttacks = 0;
                        dashAbility.SetActive(true);
                        Debug.Log("Success");
                        dashUnlocked = true;


                    }
                }

                // Make the player immovable while attacking
                
            }
            else
            {
               
                StopAttack();
               
              
             
                
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
        if(detectionZone.detectedObj.Count > 0 && !player.isDead)
        {
            isAttacking = true;
           
            player.animator.SetBool("Crow", true);

        }
        else
        {
            player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            StopAttack();
        }
        
        
       


    }

    public void StopAttack()
    {
        numAttacks = 0;
        animator.SetBool("Attack", false);
        isAttacking = false;
        player.animator.SetBool("Crow", false);
        
    }
    
    
}
