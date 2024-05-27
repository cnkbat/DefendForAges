using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisualsHandler : MonoBehaviour
{
    EnemyBehaviour enemyBehaviour;
    EnemyStats enemyStats;
    GameManager gameManager;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

    private void OnEnable()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        enemyStats = GetComponent<EnemyStats>();
        gameManager = GameManager.instance;

        enemyBehaviour.OnDamageTaken += UpdateHealthBarValue;
        enemyBehaviour.OnDeath += DisableHealthBar;

        ResetDisappearTimer();
        DisableHealthBar();
    }

    private void OnDisable()
    {
        enemyBehaviour.OnDamageTaken -= UpdateHealthBarValue;
        enemyBehaviour.OnDeath -= DisableHealthBar;
    }

    private void Update()
    {
        if (!healthBar.gameObject.activeSelf) return;

        currentHealthBarDisappearTimer -= Time.deltaTime;

        if (currentHealthBarDisappearTimer < 0)
        {
            DisableHealthBar();
        }
    }

    public void UpdateHealthBarValue()
    {
        if (enemyBehaviour.isDead) return;

        healthBar.gameObject.SetActive(true);
        healthBar.value = enemyBehaviour.GetCurrentHealth() / enemyStats.GetMaxHealth();

        ResetDisappearTimer();
    }

    private void ResetDisappearTimer()
    {
        currentHealthBarDisappearTimer = gameManager.enemyHealthBarDisappearTimer;
    }

    public void DisableHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}
