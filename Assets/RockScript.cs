using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update

    public GameObject[] dropPrefab;
    public Animator animator;
    public Collider2D collider;
    public TreeSoundManager treeSoundManager;


    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                collider.enabled = false;
                animator.SetBool("Destroyed", true);
                InventoryManager.instance.ReduceDurability();
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


    public void TreeHit(AudioClip treeHit)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = treeHit;

        // Modify pitch
        audioSource.pitch = 1.25f;

        // Modify spatial blend
        audioSource.spatialBlend = 0.4f;

        // Modify rolloff
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 2f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        // Play the audio
        audioSource.Play();
    }

    public void OnHit(float damage, Vector2 knockback)
    {

        Health -= (damage * .7f);
        
        animator.SetTrigger("Hit");
        treeSoundManager.TreeHit();
    }

    public void OnHit(float damage)
    {
        if(_health >= 0)
        {
            Health -= (damage * .7f);
           
            treeSoundManager.TreeHit();
        }
        
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        _health = maxHealth;
    }

    // Update is called once per frame
   
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

        Instantiate(dropPrefab[Random.Range(0, dropPrefab.Length)], transform.position, Quaternion.identity);


    }

    public void OnBurn(float damage, float time)
    {
        StartCoroutine(ApplyBurnDamage(damage, time));
    }

    private IEnumerator ApplyBurnDamage(float damage, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time && _health > 0)
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
