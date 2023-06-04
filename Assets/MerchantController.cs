using System.Collections;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    public GameObject[] itemsToSell;
    public float activationDuration = 5f;
    public float deactivationDuration = 2f;

    private Coroutine activationCoroutine;

    private void Start()
    {
        // Start the activation coroutine
        activationCoroutine = StartCoroutine(ActivateRandomItems());
    }

    private IEnumerator ActivateRandomItems()
    {
        while (true)
        {
            // Activate random item
            int randomActivateIndex = Random.Range(0, itemsToSell.Length);
            itemsToSell[randomActivateIndex].SetActive(true);

            // Wait for the activation duration
            yield return new WaitForSeconds(activationDuration);

            // Deactivate random item
            int randomDeactivateIndex = Random.Range(0, itemsToSell.Length);
            itemsToSell[randomDeactivateIndex].SetActive(false);

            // Wait for the deactivation duration
            yield return new WaitForSeconds(deactivationDuration);
        }
    }

    public void StopMerchant()
    {
        // Stop the activation coroutine
        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine);
        }

        // Deactivate all items
        foreach (GameObject item in itemsToSell)
        {
            item.SetActive(false);
        }
    }
}
