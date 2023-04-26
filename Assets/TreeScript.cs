using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update


    public Animator animator;
    public GameObject[] dropPrefab;
    
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                DropItem();
                Destroy(gameObject);
            }
        }
        get
        {
            return _health;
        }
    }
    public float _health = 500;

    public void OnHit(float damage, Vector2 knockback)
    {
        
        Health -= (damage * .7f);
        Debug.Log("Tree healt" + Health);
        animator.SetTrigger("Hit");
    }

    public void OnHit(float damage)
    {
        Health -= (damage * .7f);
        Debug.Log("Tree healt" + Health);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DropItem()
    {
       
            Instantiate(dropPrefab[Random.Range(1, dropPrefab.Length)], transform.position, Quaternion.identity);

        
    }
}
