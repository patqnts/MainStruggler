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
    public GameObject close;



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
       

        }

    }

    public void OpenUI()
    {
        shopUI.gameObject.SetActive(true);
        
    }
    public void CloseUI()
    {
      
        shopUI.gameObject.SetActive(false);
    }
}
