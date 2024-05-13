using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    PlayerAttack playerAttack;
    ObjectPooler objectPooler;

    [Header("Firing")]
    [SerializeField] private String bulletTag;
    [SerializeField] private Transform tipOfWeapon;

    private void OnEnable()
    {
        objectPooler = ObjectPooler.instance;
    }

    public void Attack(Transform bulletTarget, float damage, bool isPlayersBullet = false)
    {
        if (!gameObject.activeSelf) return;

        objectPooler.SpawnBulletFromPool(bulletTag, tipOfWeapon.position, bulletTarget, damage, isPlayersBullet);
    }


}
