using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDialog : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject gameOverBox;
    public Movement player;

    public string inactiveObjectName = "GameOver Placeholder";
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

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            gameOverBox.SetActive(true);
        }
    }
}
