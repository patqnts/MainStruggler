using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeQueenHealthUI : MonoBehaviour
{
    public GameObject SlimeHealthUI;
    public CircleCollider2D circleCollider2D;

    private void Start()
    {
        SlimeHealthUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SlimeHealthUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SlimeHealthUI.gameObject.SetActive(false);
        }
    }
}
