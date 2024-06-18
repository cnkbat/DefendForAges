using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisualsHandler : MonoBehaviour
{
    private EnemyDeathHandler enemyDeathHandler;
    private EnemyStats enemyStats;
    private GameManager gameManager;
    private IndicatorAssigner indicatorAssigner;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

    [Header("VFX")]
    [SerializeField] private ParticleSystem damageTakenParticle;

    private void Awake()
    {
        enemyDeathHandler = GetComponent<EnemyDeathHandler>();
        enemyStats = GetComponent<EnemyStats>();
        indicatorAssigner = GetComponentInChildren<IndicatorAssigner>();
    }
    private void OnEnable()
    {
        gameManager = GameManager.instance;

        indicatorAssigner?.OnEnableIndicator?.Invoke(PointableTypes.enemy, Color.red);

        enemyDeathHandler.OnDamageTaken += UpdateHealthBarValue;
        enemyDeathHandler.OnDamageTaken += PlayDamageTakenVFX;

        enemyDeathHandler.OnDeath += DisableHealthBar;


        StopAllVFXs();

        ResetDisappearTimer();
        DisableHealthBar();
    }

    private void OnDisable()
    {
        enemyDeathHandler.OnDamageTaken -= UpdateHealthBarValue;
        enemyDeathHandler.OnDamageTaken -= PlayDamageTakenVFX;

        enemyDeathHandler.OnDeath -= DisableHealthBar;
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
        if (enemyDeathHandler.GetIsDead()) return;

        healthBar.gameObject.SetActive(true);
        healthBar.value = enemyDeathHandler.GetCurrentHealth() / enemyStats.GetMaxHealth();

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
