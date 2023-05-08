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
   




    public string inactiveObjectName = "MainInventoryGroup"; // Reference to the inactive GameObject
    void Start()
    {
        GameObject canvas = GameObject.Find("Main User Interface");
        // Find the inactive GameObject by name
        inactiveObject = canvas.transform.Find(inactiveObjectName).gameObject;

        if (inactiveObject != null)
        {
            inactiveObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            notice.gameObject.SetActive(true);
            noticeButtonUI.gameObject.SetActive(true);
            
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

    }
    public void CloseUI()
    {
        noticeButtonUI.gameObject.SetActive(true);
        shopUI.gameObject.SetActive(false);
        if (inactiveObject != null)
        {
            inactiveObject.SetActive(false);
        }

    }
}
