using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]


public class EnemySO : ScriptableObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float damage;
    [SerializeField] int moneyValue;
    [SerializeField] float powerUpAddOnValue;
    [SerializeField] bool isBoss;
    [SerializeField] float knockbackDur;

    public float GetPowerUpAddOnValue() { return powerUpAddOnValue; }
    public float GetMoveSpeed() { return moveSpeed; }
    public void SetMoveSpeed(float speed) { moveSpeed = speed; }
    public float GetMaxHealth() { return maxHealth; }
    public void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; }
    public float GetDamage() { return damage; }
    public void SetDamage(float dmg) { damage = dmg; }
    public int GetMoneyValue() { return moneyValue; }
    public bool GetIsBoss() { return isBoss; }
    public float GetKnockbackDur() { return knockbackDur; }
    
}
