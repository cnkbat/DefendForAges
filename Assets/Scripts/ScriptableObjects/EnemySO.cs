using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]


public class EnemySO : ScriptableObject
{
    [Header("Health")]
    [SerializeField] private float maxHealth;

    [Header("Speed")]
    [SerializeField] private float moveSpeed;

    [Header("Attacking")]
    [SerializeField] private float knockbackDur;
    [SerializeField] private float attackDur;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    [Header("Earnings")]
    [SerializeField] private int moneyValue;
    [SerializeField] private int expValue;
    [SerializeField] private int meatValue;
    [SerializeField] private float powerUpAddOnValue;

    public float GetPowerUpAddOnValue() { return powerUpAddOnValue; }
    public float GetMovementSpeed() { return moveSpeed; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetDamage() { return damage; }
    public int GetMoneyValue() { return moneyValue; }
    public int GetExpValue() { return expValue; }
    public int GetMeatValue() { return meatValue; }
    public float GetKnockbackDur() { return knockbackDur; }
    public float GetAttackDur() { return attackDur; }
    public float GetAttackRange() {return attackRange;}
}
