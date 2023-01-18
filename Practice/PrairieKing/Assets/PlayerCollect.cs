using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] private BulletWeaponScript weapon;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WeaponUpgrade"))
        {
            weapon.UpdateWeapon();
            Destroy(other.gameObject);
        }
    }
}
