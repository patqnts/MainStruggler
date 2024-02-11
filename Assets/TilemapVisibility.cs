using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisibility : MonoBehaviour
{
    public TilemapRenderer tilemapRenderer;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void OnBecameVisible()
    {
       
        tilemapRenderer.enabled = true;
    }

    private void OnBecameInvisible()
    {
      
        tilemapRenderer.enabled = false;
    }
}
