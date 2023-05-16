using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visible : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject self;
    private void Start()
    {
        self.gameObject.SetActive(false);
    }
    private void OnBecameVisible()
    {
        self.gameObject.SetActive(true);
    }

    private void OnBecameInvisible()
    {
        self.gameObject.SetActive(false);
    }
}
