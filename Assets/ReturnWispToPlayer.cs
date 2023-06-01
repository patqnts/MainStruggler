using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnWispToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public FairyHolder fairy;
    void Start()
    {
        fairy = FindObjectOfType<FairyHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!= null & collision.CompareTag("Player"))
        {
            fairy.ReturnToPlayer();
        }
    }


}
