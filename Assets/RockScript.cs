using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update

    public GameObject[] dropPrefab;
    public Animator animator;
    public Collider2D collider;

    
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                collider.enabled = false;
                animator.SetBool("Destroyed", true);
                Destroy(gameObject, 1f);
                DropItem();
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
        Debug.Log("rock health" + Health);
        animator.SetTrigger("Hit");
    }

    public void OnHit(float damage)
    {
        Health -= (damage * .7f);
        Debug.Log("rock health" + Health);
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
       

            Instantiate(dropPrefab[Random.Range(0, dropPrefab.Length)], transform.position, Quaternion.identity);

        
    }
}
