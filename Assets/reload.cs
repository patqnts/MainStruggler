using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reload : MonoBehaviour
{

    Movement player;
    public GameObject button;
    private void Start()
    {
        player = FindObjectOfType<Movement>();
    }
    public void Reload()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        RuinSavePoint.PlayerDied();
        player.Respawn();
        button.SetActive(false);
        
    }
}
