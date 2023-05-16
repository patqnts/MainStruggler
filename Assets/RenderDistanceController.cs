using UnityEngine;

public class RenderDistanceController : MonoBehaviour
{
    public float maxRenderDistance = 10f;
    private float currentRenderDistance;

    private void Start()
    {
        // Set initial render distance to a value larger than the max distance
        currentRenderDistance = maxRenderDistance + 1f;

        // Invoke UpdateRenderDistance method every 0.2 seconds
        InvokeRepeating("UpdateRenderDistance", 0f, 0.2f);
    }

    private void UpdateRenderDistance()
    {
        // Find all objects with the "Renderable" tag
        GameObject[] objectsToRender = GameObject.FindGameObjectsWithTag("Ground");

        foreach (GameObject obj in objectsToRender)
        {
            // Calculate distance between player and object
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // Enable renderer if object is within current render distance, disable otherwise
            if (distance <= currentRenderDistance)
            {
                obj.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                obj.GetComponent<Renderer>().enabled = false;
            }
        }

        // Gradually increase current render distance until it reaches max distance
        currentRenderDistance = Mathf.Min(currentRenderDistance + 1f, maxRenderDistance);
    }
}
