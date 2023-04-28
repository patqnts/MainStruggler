using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TransparencyOnTriggerEnter : MonoBehaviour
{
    public float alpha = 0.5f; // Change this value to adjust the transparency

    private Material material;
    private Collider2D parentCollider;

    void Start()
    {
        material = transform.parent.GetComponent<Renderer>().material;
        parentCollider = transform.parent.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color color = material.color;
            color.a = 1f;
            material.color = color;
        }
    }
}
