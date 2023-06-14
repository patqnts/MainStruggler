using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D coll;
    public string Target = "Player";


    public GameObject notice;
    public GameObject noticeButtonUI;
    public GameObject shopUI;

    private GameObject inactiveObject;
    private GameObject InventoryBag;
    public AudioSource mooSound;




    public string inactiveObjectName = "MainInventoryGroup"; // Reference to the inactive GameObject
    public string inventory = "ShowMainInventoryButton";
    void Start()
    {
        GameObject canvas = GameObject.Find("Main User Interface");
        
        // Find the inactive GameObject by name
        inactiveObject = canvas.transform.Find(inactiveObjectName).gameObject;
        InventoryBag = canvas.transform.Find(inventory).gameObject;

        if (inactiveObject != null)
        {
            inactiveObject.SetActive(false);
        }
        if (InventoryBag != null)
        {
            InventoryBag.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            notice.gameObject.SetActive(true);
            noticeButtonUI.gameObject.SetActive(true);
            mooSound.Play();


        }
        else if (collision.CompareTag("Tree") || collision.CompareTag("Rock"))
        {
            Destroy(collision.gameObject);

        }
        else if (collision.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemyRigidbody.AddForce(knockbackDirection * 10f, ForceMode2D.Impulse);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        

        if (collision.gameObject.CompareTag("Player"))
        {
            notice.gameObject.SetActive(false);
            noticeButtonUI.gameObject.SetActive(false); 
            shopUI.gameObject.SetActive(false);

            if (inactiveObject != null)
            {
                inactiveObject.SetActive(false);
            }
            if (InventoryBag != null)
            {
                InventoryBag.SetActive(true);
            }

        }

    }

    public void OpenUI()
    {
        if (inactiveObject != null)
        {
            inactiveObject.SetActive(true); // Activate the inactive object
        }
        shopUI.gameObject.SetActive(true);
        noticeButtonUI.gameObject.SetActive(false);
        InventoryBag.SetActive(false);

    }
    public void CloseUI()
    {
        noticeButtonUI.gameObject.SetActive(true);
        shopUI.gameObject.SetActive(false);
        if (inactiveObject != null)
        {
            inactiveObject.SetActive(false);
            InventoryBag.SetActive(true);
        }

    }
}
