using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGhost : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost = false;

    // Reference to the sprite renderer of the character
    private SpriteRenderer characterSpriteRenderer;

    void Start()
    {
        ghostDelaySeconds = ghostDelay;

        // Get the sprite renderer of the character
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);

                // Set the scale of the ghost to match the character's scale
                currentGhost.transform.localScale = transform.localScale;

                // Set the sprite of the ghost to match the character's sprite
                SpriteRenderer ghostSpriteRenderer = currentGhost.GetComponent<SpriteRenderer>();
                ghostSpriteRenderer.sprite = characterSpriteRenderer.sprite;

                // Flip the ghost sprite if the character is facing left
                ghostSpriteRenderer.flipX = characterSpriteRenderer.flipX;

                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1f);
            }
        }
    }
}
