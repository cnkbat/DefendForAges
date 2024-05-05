using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStats : MonoBehaviour
{

    GameManager gameManager;
    EnemyBehaviour enemyBehaviour;

    public EnemySO EnemySO;
    public float currentHealth;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] float maxHealth;


    [Header("Attacking")]
    public float attackSpeed;
    public float attackDur;
    public float knockbackDur;
    public float currentDamage;

    [Header("Earnings")]
    private int moneyValue;
    private int expValue;
    private float powerUpAddOnValue;
   

    [Header("Health Bar")]
    [SerializeField] Slider healthBar;
    [SerializeField] List<Image> healthBarImages;

    public void EnemySpawned()
    {
        gameManager = GameManager.instance;
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();

        SetEnemySOValues();
        // UpdateHealthBar();
        //  ResetEnemy();

    }

    private void SetEnemySOValues()
    {
        powerUpAddOnValue = EnemySO.GetPowerUpAddOnValue();
        knockbackDur = EnemySO.GetKnockbackDur();
        currentMoveSpeed = EnemySO.GetMoveSpeed();
        currentDamage = EnemySO.GetDamage();
        moneyValue = EnemySO.GetMoneyValue();
        expValue = EnemySO.GetExpValue();
        maxHealth = EnemySO.GetMaxHealth();

        RefillHealth();
    }




    #region Health Related

    private void RefillHealth()
    {
        currentHealth = maxHealth;
    }

    private void UpdateHealthBar()
    {

        if (!healthBar) return;
        healthBar.gameObject.SetActive(true);

        if (currentHealth < maxHealth)
        {
            for (int i = 0; i < healthBarImages.Count; i++)
            {
                healthBarImages[i].DOFade(255f, 0);
            }

            healthBar.value = currentHealth / maxHealth;

            for (int i = 0; i < healthBarImages.Count; i++)
            {
                healthBarImages[i].DOFade(0, 1);
            }
        }


    }

    #endregion

    #region Getters & Setters
    public float GetDamage()
    {
        return currentDamage;
    }
    public int GetMoneyValue()
    {
        return moneyValue;
    }
    public int GetExpValue()
    {
        return expValue;
    }
    public float GetKnockbackDuration()
    {
        return knockbackDur;
    }
    public Slider GetHealthBar()
    {
        return healthBar;
    }
    public float GetPowerUpValue()
    {
        return powerUpAddOnValue;
    }
    #endregion

}
