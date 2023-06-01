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

    public GameObject projectilePrefab; // The prefab of the projectile to be dropped
    private bool isDroppingProjectiles = false; // Flag to track if the fairy is currently dropping projectiles
    private Coroutine projectileCoroutine; // Coroutine reference to stop the projectile dropping

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
        if (tite.sprite != null && cycle.isNight)
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
            if (wispLight2D != null)
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
            speed = 0.1f;
            StartDroppingProjectiles(); // Start dropping projectiles
        }
        else
        {
            Debug.LogWarning("BlackTarget game object not found in the hierarchy.");
        }
    }

    public void WitchTarget()
    {
        GameObject witchTargetObject = GameObject.Find("WitchTarget"); // Find the game object named "WitchTarget"

        if (witchTargetObject != null)
        {
            // Set the target to the transform of the witchTargetObject
            target = witchTargetObject.transform;
            speed = 0.1f;
            StartDroppingProjectiles(); // Start dropping projectiles
        }
        else
        {
            Debug.LogWarning("WitchTarget game object not found in the hierarchy.");
        }
    }

    public void MerchantTarget()
    {
        GameObject merchantTargetObject = GameObject.Find("MerchantTarget"); // Find the game object named "MerchantTarget"

        if (merchantTargetObject != null)
        {
            // Set the target to the transform of the merchantTargetObject
            target = merchantTargetObject.transform;
            speed = 0.1f;
            StartDroppingProjectiles(); // Start dropping projectiles
        }
        else
        {
            Debug.LogWarning("MerchantTarget game object not found in the hierarchy.");
        }
    }

    public void ReturnToPlayer()
    {
        GameObject fairyFollow = GameObject.Find("fairy-follow"); // Find the game object named "fairy-follow"

        if (fairyFollow != null)
        {
            // Set the target to the transform of the fairyFollow object
            target = fairyFollow.transform;
            speed = 1f;
            StopDroppingProjectiles(); // Stop dropping projectiles
        }
        else
        {
            Debug.LogWarning("fairy-follow game object not found in the hierarchy.");
        }
    }

    private void StartDroppingProjectiles()
    {
        if (!isDroppingProjectiles)
        {
            // Start the coroutine to drop projectiles every 1 second
            projectileCoroutine = StartCoroutine(DropProjectilesCoroutine());
            isDroppingProjectiles = true;
        }
    }

    private void StopDroppingProjectiles()
    {
        if (isDroppingProjectiles)
        {
            // Stop the coroutine and reset the flags
            StopCoroutine(projectileCoroutine);
            isDroppingProjectiles = false;
        }
    }

    private IEnumerator DropProjectilesCoroutine()
{
    while (true)
    {
        // Instantiate the projectile at the current position of the fairy
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Destroy the projectile after 5 seconds
        Destroy(projectile, 8f);

        yield return new WaitForSeconds(1f); // Wait for 1 second before dropping the next projectile
    }
}

}
