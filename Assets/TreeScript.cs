using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public TreeSoundManager treeSoundManager;

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
                InventoryManager.instance.ReduceDurability();
                collider.enabled = false;
                animator.SetBool("Destroyed", true);
                DestroyTree();
               
                
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
        treeSoundManager.TreeHit();
        Health -= (damage * .7f);
        Debug.Log("Tree healt" + Health);
        animator.SetTrigger("Hit");
    }

    public void OnHit(float damage)
    {
        if (_health >= 0)
        {
            treeSoundManager.TreeHit();
            Health -= (damage * .7f);
            Debug.Log("rock health" + Health);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        _health = maxHealth;

    }

    private void DestroyTree()
    {
        treeSoundManager.TreeDestroyed();
        collider.enabled = false;
        animator.SetBool("Destroyed", true);
        DropItem();
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

        if (dropPrefab != null)
        {
            foreach (var prefab in dropPrefab)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }

    }

    private bool isBurning = false;
    public void OnBurn(float damage, float time)
    {
        if (!isBurning && _health > 0)
        {
            StartCoroutine(ApplyBurnDamage(damage, time));
        }

        Debug.Log("BURRRRN");
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        isBurning = true;
        float elapsedTime = 0f;
        float multiplier = 20f;
         
        while (elapsedTime < time && _health > 0)
        {

            float increasedDamage = damage * multiplier;
            
            yield return new WaitForSeconds(1f);
            OnHit(increasedDamage);
            Debug.Log(increasedDamage);

            elapsedTime += 1f;
            multiplier++;
        }
        isBurning = false;
    }


    public void OnDark(float time)
    {
        throw new System.NotImplementedException();
    }
}
