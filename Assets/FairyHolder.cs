using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FairyHolder : MonoBehaviour
{
    public Transform target; // the target to follow
    public float distance = 1f; // the distance to maintain from the target
    public float speed = 5f; // the speed at which to follow the target

    private Vector3 targetPosition; // the position to move towards
    public Light2D wispLight2D; // Reference to the Light2D component
    public SpriteRenderer tite;

    public DayNightCycles cycle;
    void Start()
    {
        // Get the Light2D component from the FairyHolder GameObject
        wispLight2D = GetComponent<Light2D>();
        tite = GetComponent<SpriteRenderer>();
        cycle = FindObjectOfType<DayNightCycles>();
    }

    void Update()
    {
        // calculate the target position based on the target and distance
        targetPosition = target.position - transform.forward * distance;

        // use lerp to smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the Sprite Renderer is null
        if (tite.sprite != null&& cycle.isNight)
        {
            // Set the intensity of the Light2D component to 0
            if (wispLight2D != null)
            {          
                    wispLight2D.intensity = 1f;
            }
        }
        else
        {
            // Set the intensity of the Light2D component to its original value (1 in this example)
            if (wispLight2D != null && !cycle.isNight)
            {       
                wispLight2D.intensity = 0f;

            }
        }
    }

    public void BlackTarget()
    {
        GameObject blackTargetObject = GameObject.Find("BlackTarget"); // Find the game object named "BlackTarget"

        if (blackTargetObject != null)
        {
            // Set the target to the transform of the blackTargetObject
            target = blackTargetObject.transform;
        }
        else
        {
            Debug.LogWarning("BlackTarget game object not found in the hierarchy.");
        }
    }

    public void ReturnToPlayer()
    {
        GameObject fairyFollow = GameObject.Find("fairy-follow"); // Find the game object named "BlackTarget"

        if (fairyFollow != null)
        {
            // Set the target to the transform of the blackTargetObject
            target = fairyFollow.transform;
        }
        else
        {
            Debug.LogWarning("BlackTarget game object not found in the hierarchy.");
        }
    }
}
