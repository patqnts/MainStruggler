using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class reload : MonoBehaviour
{

    Movement player;
    public GameObject button;
    public GhostEnemyAI dogo;
    public NPCManager npcManager;
    public LoadSystem load;
    public GameObject inventoryBag;
    private void Start()
    {
        player = FindObjectOfType<Movement>();
        dogo = FindObjectOfType<GhostEnemyAI>();
    }

    private void Update()
    {
       
    }
    private void OnEnable()
    {
        dogo = FindObjectOfType<GhostEnemyAI>();
         load = FindObjectOfType<LoadSystem>();
    }
    public void Reload()
    {

       // CrazyAds.Instance.beginAdBreak();
        RuinSavePoint.PlayerDied();
        player.Respawn();
        button.SetActive(false);
        inventoryBag.SetActive(false);
        if (dogo != null)
        {
            Destroy(dogo.gameObject);
            npcManager.dogspawns = 0;
            //npcManager.SpawnDogo();
            Instantiate(npcManager.Dogo, npcManager.dogoTotemSavedPos, Quaternion.identity);
        }
        
    }
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
