using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update


    public Animator animator;
    public GameObject[] dropPrefab;
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
                DestroyTree();
                DropItem();
                
            }
        }
        get
        {
            return _health;
        }
    }
    public float _health = 500;
    public float maxHealth = 500;
    
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
        _health = maxHealth;

    }

    private void DestroyTree()
    {
        collider.enabled = false;
        animator.SetBool("Destroyed", true);
       
        StartCoroutine(RespawnTree());
    }

    private IEnumerator RespawnTree()
    {
        yield return new WaitForSeconds(300f);

        _health = maxHealth;
        collider.enabled = true;
        animator.SetBool("Destroyed", false);
    }
    private void DropItem()
    {
       
            Instantiate(dropPrefab[Random.Range(1, dropPrefab.Length)], transform.position, Quaternion.identity);

        
    }

    private bool isBurning = false;
    public void OnBurn(float damage, float time)
    {
        if (!isBurning)
        {
            StartCoroutine(ApplyBurnDamage(damage, time));
        }

        Debug.Log("BURRRRN");
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        isBurning = true;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            yield return new WaitForSeconds(1f);

            OnHit(damage);
            Debug.Log("BURRRRN");

            elapsedTime += 1f;
        }
        isBurning = false;
    }
}
