using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerAttack playerAttack;
    ObjectPooler objectPooler;

    [Header("Firing")]
    [SerializeField] private String bulletTag;
    [SerializeField] private Transform tipOfWeapon;
    float damage;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        objectPooler = ObjectPooler.instance;
        playerAttack = FindObjectOfType<PlayerAttack>();

        playerAttack.OnAttack += Attack;
    }

    private void OnDisable()
    {
        playerAttack.OnAttack -= Attack;
    }

    private void Attack(Transform newTransform)
    {
        damage = playerStats.GetDamage();

        objectPooler.SpawnFromPool(bulletTag, tipOfWeapon.position, newTransform);

    }


}
