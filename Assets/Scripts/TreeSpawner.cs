using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public int treeCount;
    void Start()
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
            {
                GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);
                tree.transform.SetParent(transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
