using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadControls : MonoBehaviour
{
    // Start is called before the first frame update
    string w = "w";
    public void onW()
    {
        Input.GetButton(w);

        Debug.Log("press");
    }
    public void onS()
    {
        Input.GetKeyDown(KeyCode.S);
        Debug.Log("press");
    }
    public void onA()
    {
        Input.GetKeyDown(KeyCode.A);
        Debug.Log("press");
    }
    public void onD()
    {
        Input.GetKeyDown(KeyCode.D);
        Debug.Log("press");
    }

}
