using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WaterScript : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private float pushForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 pushDirection = groundTilemap.transform.position - transform.position;
            rb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
        }
    }
}






