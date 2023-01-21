using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] private BulletWeaponScript weapon;
    [SerializeField] private PlayerScript player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WeaponUpgrade"))
        {
            weapon.UpdateWeapon();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("AddHealth"))
        {
            player.SetHealth(player.health + 1);
            Destroy(other.gameObject);
        }
    }
}
