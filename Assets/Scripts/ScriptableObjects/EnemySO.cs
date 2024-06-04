using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]


public class EnemySO : ScriptableObject
{
    [Header("Health")]
    [SerializeField] private float maxHealth;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float knockbackDur;

    [Header("Attacking")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDur;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    [Header("Earnings")]
    [SerializeField] private float coinValue;
    [SerializeField] private float expValue;
    [SerializeField] private float meatValue;
    [SerializeField] private float powerUpAddOnValue;

    public float GetPowerUpAddOnValue() { return powerUpAddOnValue; }
    public float GetMovementSpeed() { return moveSpeed; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetDamage() { return damage; }
    public float GetMoneyValue() { return coinValue; }
    public float GetExpValue() { return expValue; }
    public float GetMeatValue() { return meatValue; }
    public float GetKnockbackDur() { return knockbackDur; }
    public float GetAttackDur() { return attackDur; }
    public float GetAttackRange() { return attackRange; }
    public float GetAttackSpeed() { return attackSpeed; }
}
