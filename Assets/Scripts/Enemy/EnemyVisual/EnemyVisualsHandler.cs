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

    [Header("VFX")]
    private ParticleSystem damageTakenParticle;

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        enemyStats = GetComponent<EnemyStats>();
        damageTakenParticle = GameObject.Find("DamageTakenParticle").GetComponent<ParticleSystem>();

        enemyBehaviour.OnDamageTaken += UpdateHealthBarValue;
        enemyBehaviour.OnDamageTaken += PlayDamageTakenVFX;

        enemyBehaviour.OnDeath += DisableHealthBar;


        StopAllVFXs();

        ResetDisappearTimer();
        DisableHealthBar();
    }

    private void OnDisable()
    {
        enemyBehaviour.OnDamageTaken -= UpdateHealthBarValue;
        enemyBehaviour.OnDamageTaken -= PlayDamageTakenVFX;

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

    #region Health Bar
    public void UpdateHealthBarValue()
    {
        if (enemyBehaviour.isDead) return;

        healthBar.gameObject.SetActive(true);
        healthBar.value = enemyBehaviour.GetCurrentHealth() / enemyStats.GetMaxHealth();

        ResetDisappearTimer();
    }

    private void ResetDisappearTimer()
    {
        currentHealthBarDisappearTimer = gameManager.healthBarDisappearTimer;
    }

    public void DisableHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
    #endregion

    #region VFX

    public void PlayDamageTakenVFX()
    {

        if (damageTakenParticle.isPlaying)
        {
            damageTakenParticle?.Stop();
        }

        damageTakenParticle?.Play();

    }


    public void StopAllVFXs()
    {
        damageTakenParticle?.Stop();
    }

    #endregion
}
