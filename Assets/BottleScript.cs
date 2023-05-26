using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour, IDamageable
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
                animator.SetBool("Broken", true);
                DropItem();

            }
        }
        get
        {
            return _health;
        }
    }
    public float _health = 1;
    public float maxHealth = 1;

    public void OnHit(float damage, Vector2 knockback)
    {

        Health -= damage;
        
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        _health = maxHealth;
    }

    // Update is called once per frame

   

    
    private void DropItem()
    {

        Instantiate(dropPrefab[Random.Range(0, dropPrefab.Length)], transform.position, Quaternion.identity);


    }

    public void OnBurn(float damage, float time)
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            yield return new WaitForSeconds(1f);

            OnHit(damage);

            elapsedTime += 1f;
        }
    }



    public void OnDark(float time)
    {
        throw new System.NotImplementedException();
    }
}
