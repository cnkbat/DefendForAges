using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisualsHandler : MonoBehaviour
{
    GameManager gameManager;
    LevelSystem levelSystem;
    PlayerStats playerStats;
    PlayerDeathHandler playerDeathHandler;
    MMFeedbacks feedBacks;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

    [Header("VFX")]
    [SerializeField] private ParticleSystem playerDeathParticle;
    [SerializeField] private ParticleSystem damageTakenParticle;

    [SerializeField] private ParticleSystem levelUpParticle;
    [SerializeField] private ParticleSystem lifeStealParticle;
    [SerializeField] private ParticleSystem[] powerupParticles;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerDeathHandler = GetComponent<PlayerDeathHandler>();
        levelSystem = GetComponent<LevelSystem>();
        feedBacks = GetComponentInChildren<MMFeedbacks>();

        if (feedBacks != null)
        {
            feedBacks.Initialization();
        }
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;

        // Health Bar
        playerDeathHandler.OnDamageTaken += UpdateHealthBarValue;
        playerStats.OnLifeStolen += UpdateHealthBarValue;

        // VFX
        playerStats.OnPlayerKilled += PlayDeathParticle;
        levelSystem.OnLevelUp += PlayLevelupParticle;
        playerStats.OnLifeStolen += PlayLifeStealParticle;
        playerStats.OnPowerUpEnabled += PlayPowerupParticle;
        playerStats.OnPowerUpDisabled += StopPowerupParticle;
        playerDeathHandler.OnDamageTaken += PlayDamageTakenVisuals;



        ResetDisappearTimer();
        DisableHealthBar();
    }

    private void OnDisable()
    {
        // Health Bar
        playerDeathHandler.OnDamageTaken -= UpdateHealthBarValue;
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
        if (playerDeathHandler.GetIsDead()) return;

        healthBar.gameObject.SetActive(true);
        healthBar.value = playerDeathHandler.GetCurrentHealth() / playerStats.GetMaxHealth();

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
    public void PlayDamageTakenVisuals()
    {
        PlayParticle(damageTakenParticle);

        if (feedBacks.IsPlaying)
        {
            feedBacks?.StopFeedbacks();
        }

        feedBacks?.PlayFeedbacks();
    }

    #region Powerup

    public void PlayPowerupParticle()
    {
        for(int i = 0; i < powerupParticles.Length; i++)
        {
            PlayParticle(powerupParticles[i]);
        }
    }

    public void StopPowerupParticle()
    {
        for (int i = 0; i < powerupParticles.Length; i++)
        {
            powerupParticles[i].Stop();
        }
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
