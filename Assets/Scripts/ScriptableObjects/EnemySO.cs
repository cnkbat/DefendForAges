using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]


public class EnemySO : ScriptableObject
{
    [Header("Health")]
    [SerializeField] float maxHealth;

    [Header("Speed")]
    [SerializeField] float moveSpeed;

    [Header("Attacking")]
    [SerializeField] float knockbackDur;
    [SerializeField] float attackDur;
    [SerializeField] float damage;

    [Header("Earnings")]
    [SerializeField] int moneyValue;
    [SerializeField] int expValue;
    [SerializeField] int meatValue;
    [SerializeField] float powerUpAddOnValue;

    public float GetPowerUpAddOnValue() { return powerUpAddOnValue; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetDamage() { return damage; }
    public int GetMoneyValue() { return moneyValue; }
    public int GetExpValue() { return expValue; }
    public int GetMeatValue() { return meatValue; }
    public float GetKnockbackDur() { return knockbackDur; }
    public float GetAttackDur() { return attackDur; }

}
