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
        Debug.Log("water vis");
        tilemapRenderer.enabled = true;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("water inv");
        tilemapRenderer.enabled = false;
    }
}
