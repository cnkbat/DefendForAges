using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.EditorTools;

public class EnemyStats : MonoBehaviour
{

    GameManager gameManager;
    EnemyBehaviour enemyBehaviour;

    public EnemySO enemySO;

    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float maxHealth;

    [Tooltip("Attacking")]
    public float attackSpeed;
    public float attackDur;
    public float knockbackDur;
    public float currentDamage;

    [Header("Earnings")]
    private int moneyValue;
    private int expValue;
    private int meatValue;
    private float powerUpAddOnValue;

    [Header("Health Bar")]
    [SerializeField] Slider healthBar;
    [SerializeField] List<Image> healthBarImages;

    public void EnemySpawned()
    {
        gameManager = GameManager.instance;
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();

        SetEnemySOValues();
    }

    private void SetEnemySOValues()
    {
        powerUpAddOnValue = enemySO.GetPowerUpAddOnValue();
        knockbackDur = enemySO.GetKnockbackDur();
        currentMoveSpeed = enemySO.GetMoveSpeed();
        currentDamage = enemySO.GetDamage();
        moneyValue = enemySO.GetMoneyValue();
        expValue = enemySO.GetExpValue();
        meatValue = enemySO.GetMeatValue();
        maxHealth = enemySO.GetMaxHealth();

    }

    #region Health Related

    /*  private void UpdateHealthBar()
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


      } */

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
    public int GetMeatValue()
    {
        return meatValue;
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    #endregion

}
