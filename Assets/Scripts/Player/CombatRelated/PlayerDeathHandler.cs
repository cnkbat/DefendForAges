using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : EnemyTarget
{
    private bool isDead;

    private void Awake()
    {
        isPlayer = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerStats.OnRevivePlayer += RevivePlayer;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playerStats.OnRevivePlayer -= RevivePlayer;
    }

    override protected void Start()
    {
        base.Start();
    }

    public void Kill()
    {
        if (isDestroyed) return;
        if (isDead) return;

        SetIsDead(true);

        Time.timeScale = 0.5f; // sonra balancelicaz


        isDestroyed = true;
        isTargetable = true;

        playerStats.OnPlayerKilled?.Invoke();
        OnTargetDestroyed?.Invoke();
    }


    private void RevivePlayer()
    {

        SetIsDead(false);

        transform.position = gameManager.allCities[playerStats.GetCityIndex()].GetRevivePoint().position;
        isDestroyed = false;
        isTargetable = true;
        isDead = false;

        Time.timeScale = 1;

        playerStats?.OnPlayerRevived.Invoke();
    }

    public override void TakeDamage(float dmg)
    {
        if (gameManager.isGameFreezed) return;
        if (isDestroyed) return;

        if (isDead) return;

        base.TakeDamage(dmg);

        currentHealth -= dmg;
        Debug.Log("player took damage = " + currentHealth);

        OnDamageTaken?.Invoke();

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public bool IncrementCurrentHealth(float value)
    {

        if (currentHealth >= playerStats.GetMaxHealth())
        {
            return false;
        }
        else
        {
            currentHealth += value;

            if (currentHealth > playerStats.GetMaxHealth())
            {
                playerStats.FillCurrentHealth();
            }

            return true;
        }

    }

    #region Getters & Setters
    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
    public void SetIsDead(bool dead)
    {
        isDead = dead;
    }

    #endregion

}
