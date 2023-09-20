using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    // Start is called before the first frame update
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
