using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeQueenHealthUI : MonoBehaviour
{
    public GameObject SlimeHealthUI;
    public CircleCollider2D circleCollider2D;
    private bool isDelayedDeactivation = false;
    private float delayDuration = 3f;
    private float timer = 0f;
    public List<Collider2D> detectedObj = new List<Collider2D>();
    //public Animator animator;

    private void Start()
    {
      // animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (detectedObj.Count > 0)
        {
            SlimeHealthUI.SetActive(true);

            isDelayedDeactivation = false; // Reset the delay flag
        }
        else if (!isDelayedDeactivation)
        {
            // Start the delay timer
            timer += Time.deltaTime;
            if (timer >= delayDuration)
            {
                Animator slimeHealthAnimator = SlimeHealthUI.GetComponent<Animator>();
                if (slimeHealthAnimator != null)
                {
                    slimeHealthAnimator.SetTrigger("Disappear");
                   
                    StartCoroutine(DeactivateSlimeHealthUI());
                    isDelayedDeactivation = true;
                    timer = 0f; // Reset the timer
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            SlimeHealthUI.gameObject.SetActive(true);
            detectedObj.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObj.Remove(collision);
       
    }

    private IEnumerator DeactivateSlimeHealthUI()
    {
      
        yield return new WaitForSeconds(1.5f); // Wait for 1 second

        SlimeHealthUI.gameObject.SetActive(false);
    }
}
