using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDialog : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject gameOverBox;
    public GameObject TestGroup;
    public Movement player;

    public string inactiveObjectName = "reload";
    public string inactiveObjectInventory = "MainInventoryGroup";
    void Start()
    {
        player = FindObjectOfType<Movement>();
        GameObject canvas = GameObject.Find("Main User Interface");
        // Find the inactive GameObject by name
        gameOverBox = canvas.transform.Find(inactiveObjectName).gameObject;
        

        if (gameOverBox != null)
        {
            gameOverBox.SetActive(false);
        }
    }
    private bool escOpen;
    // Update is called once per frame
    private bool isMainMenuShown = false;

    void Update()
    {
        if (player.isDead)
        {
            player.DropItemPlayer();

            // All items have been dropped, so show the game over box
            gameOverBox.SetActive(true);
            InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            TestGroup.SetActive(true);
           
        }
        
       
    }

}
