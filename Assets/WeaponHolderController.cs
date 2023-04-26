using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    public GameObject attackHitbox;
    public GameObject weaponPrefab;

    public void Attack()
    {
        // Instantiate the weapon prefab
        GameObject weaponInstance = Instantiate(weaponPrefab, transform.position, transform.rotation);

        // Get the damage value from the weapon's scriptable object
        SwordScript weaponScript = weaponInstance.GetComponent<SwordScript>();
        float damageValue = weaponScript.damage;

        // Update the damage value of the attack hitbox
       

        // Destroy the weapon instance
        Destroy(weaponInstance);
    }
}
