using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    public float distanceThreshold = 1f; // Change this value to adjust the distance threshold
    private float angleThreshold = 90f; // Change this value to adjust the angle threshold
    public float alpha = 0.5f; // Change this value to adjust the transparency

    private Material material;
    private GameObject player;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 playerToObj = transform.position - player.transform.position;
        float distance = playerToObj.magnitude;
        float angle = Vector3.Angle(playerToObj, player.transform.forward);

        if (distance <= distanceThreshold && angle >= 90)
        {
            Color color = material.color;
            color.a = alpha;
            material.color = color;
            Debug.Log(angle);
        }
        else
        {
            Color color = material.color;
            color.a = 1f;
            material.color = color;
        }
    }
}

