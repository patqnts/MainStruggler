using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform target2; // The target position to move the camera towards
    public float movementDuration = 1f;  // The duration of the camera movement

    public void MoveCameraToTarget()
    {
        StartCoroutine(MoveCameraSmoothly());
    }
    public void MoveCameraToTarget2()
    {
        StartCoroutine(MoveCameraSmoothlys());
    }
    private IEnumerator MoveCameraSmoothly()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            // Calculate the current position using Lerp
            float t = elapsedTime / movementDuration;
            Vector3 newPosition = Vector3.Lerp(startPosition, target.position, t);

            // Set the camera position to the new position
            transform.position = newPosition;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Set the final camera position to the target position
        transform.position = target.position;
    }

    private IEnumerator MoveCameraSmoothlys()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            // Calculate the current position using Lerp
            float t = elapsedTime / movementDuration;
            Vector3 newPosition = Vector3.Lerp(startPosition, target2.position, t);

            // Set the camera position to the new position
            transform.position = newPosition;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Set the final camera position to the target position
       // transform.position = target.position;
    }
}
