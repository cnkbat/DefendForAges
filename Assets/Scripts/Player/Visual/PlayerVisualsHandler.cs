using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisualsHandler : MonoBehaviour
{
    GameManager gameManager;
    LevelSystem levelSystem;
    PlayerStats playerStats;
    DeathHandler deathHandler;


    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

    [Header("VFX")]
    private ParticleSystem playerDeathParticle;
    private ParticleSystem levelUpParticle;
    private ParticleSystem lifeStealParticle;
    private ParticleSystem powerupParticle;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        deathHandler = GetComponent<DeathHandler>();
        levelSystem = GetComponent<LevelSystem>();
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;

        playerDeathParticle = GameObject.Find("PlayerDeathParticle").GetComponent<ParticleSystem>();
        levelUpParticle = GameObject.Find("LevelUpParticle").GetComponent<ParticleSystem>();
        lifeStealParticle = GameObject.Find("LifeStealParticle").GetComponent<ParticleSystem>();
        powerupParticle = GameObject.Find("PowerupParticle").GetComponent<ParticleSystem>();

        // Health Bar
        deathHandler.OnDamageTaken += UpdateHealthBarValue;
        playerStats.OnLifeStolen += UpdateHealthBarValue;

        // VFX
        playerStats.OnPlayerKilled += PlayDeathParticle;
        levelSystem.OnLevelUp += PlayLevelupParticle;
        playerStats.OnLifeStolen += PlayLifeStealParticle;
        playerStats.OnPowerUpEnabled += PlayPowerupParticle;
        playerStats.OnPowerUpDisabled += StopPowerupParticle;



        ResetDisappearTimer();
        DisableHealthBar();
    }

    private void OnDisable()
    {
        // Health Bar
        deathHandler.OnDamageTaken -= UpdateHealthBarValue;
        playerStats.OnLifeStolen -= UpdateHealthBarValue;

        // VFX
        playerStats.OnPlayerKilled -= PlayDeathParticle;
        levelSystem.OnLevelUp -= PlayLevelupParticle;
        playerStats.OnLifeStolen -= PlayLifeStealParticle;
         playerStats.OnPowerUpEnabled -= PlayPowerupParticle;
        playerStats.OnPowerUpDisabled -= StopPowerupParticle;
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
        if (playerStats.GetIsDead()) return;

        healthBar.gameObject.SetActive(true);
        healthBar.value = deathHandler.GetCurrentHealth() / playerStats.GetMaxHealth();

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

    public void PlayDeathParticle()
    {
        PlayParticle(playerDeathParticle);
    }

    public void PlayLevelupParticle()
    {
        PlayParticle(levelUpParticle);
    }

    public void PlayLifeStealParticle()
    {
        PlayParticle(lifeStealParticle);
    }

    #region Powerup

    public void PlayPowerupParticle()
    {
        PlayParticle(powerupParticle);
    }

    public void StopPowerupParticle()
    {
        powerupParticle.Stop();
    }

    #endregion

    public void PlayParticle(ParticleSystem particleToPlay)
    {
        if (particleToPlay == null)
        {
            Debug.LogWarning(name + " particle is null");

            return;
        }

        if (particleToPlay.isPlaying)
        {
            particleToPlay.Stop();
        }

        particleToPlay.Play();
    }
    #endregion

}
