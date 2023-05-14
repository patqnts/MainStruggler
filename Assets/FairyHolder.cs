using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyHolder : MonoBehaviour
{
    public Transform target; // the target to follow
    public float distance = 1f; // the distance to maintain from the target
    public float speed = 5f; // the speed at which to follow the target

    private Vector3 targetPosition; // the position to move towards

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // calculate the target position based on the target and distance
        targetPosition = target.position - transform.forward * distance;

        // use lerp to smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
