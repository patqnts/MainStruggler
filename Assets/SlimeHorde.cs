using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHorde : MonoBehaviour
{
    public GameObject slimePrefab; // Prefab to instantiate
    public int[] roundPrefabCounts = { 3, 6, 10 }; // Number of slimes to instantiate for each round
    public Animator animator; // Reference to the animator

    private bool hasCollided = false; // Flag to track collision
    private int currentRound = 0; // Current round index
    private int instantiatedPrefabs = 0; // Number of prefabs instantiated for the current round
    private int destroyedPrefabs = 0; // Number of prefabs destroyed for the current round
    private NPCManager npcManager;
    public GameObject reward;

    private void Start()
    {
        animator = GetComponent<Animator>();
        npcManager = FindObjectOfType<NPCManager>();
    }

    private void Update()
    {
        if (hasCollided && !animator.GetCurrentAnimatorStateInfo(0).IsName("Triggered"))
        {
            StartCoroutine(InstantiateSlimesAfterDelay());
            hasCollided = false;
        }

        CheckRoundCompletion();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Trigger");
            hasCollided = true;
        }
    }

    private IEnumerator InstantiateSlimesAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        int prefabCount = roundPrefabCounts[currentRound];

        for (int i = 0; i < prefabCount; i++)
        {
            GameObject slime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            slime.transform.parent = transform; // Set the parent as the current SlimeHorde instance
            instantiatedPrefabs++;
        }
    }

    private void CheckRoundCompletion()
    {
        int prefabCount = roundPrefabCounts[currentRound];

        if (destroyedPrefabs == prefabCount)
        {
            destroyedPrefabs = 0;
            currentRound++; // Increment the current round index

            if (currentRound >= roundPrefabCounts.Length)
            {
                // All rounds completed, disable the SlimeHorde
                npcManager.OnFlyDestroyed();
                GameObject rewards = Instantiate(reward, transform.position, Quaternion.identity);
                Destroy(gameObject, 1f);
            }
            else
            {
                instantiatedPrefabs = 0; // Reset the instantiated prefab count for the new round
                StartCoroutine(InstantiateSlimesAfterDelay());
            }
        }
    }

    public void PrefabDestroyed()
    {
        destroyedPrefabs++;
    }
}
