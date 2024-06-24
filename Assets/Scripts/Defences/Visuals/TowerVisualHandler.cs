using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerVisualHandler : AttackerDefencesVisualHandler
{
    TowerBehaviour towerBehaviour;

    [Header("Tower VFXs")]
    [SerializeField] private List<ParticleSystem> recoveryParticles = new List<ParticleSystem>();
    [SerializeField] private ParticleSystem reviveParticle;

    [Header("Health Bar Poses")]
    [SerializeField] private List<float> healthBarHeights;

    protected override void Awake()
    {
        base.Awake();
        towerBehaviour = GetComponent<TowerBehaviour>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        towerBehaviour.OnRecoveryDone += PlayRecoveryParticles;
        playerStats.OnWaveWon += CheckIndicator;
        defencesBehaviourBase.OnRepairDone += CheckIndicator;
        defencesBehaviourBase.OnRepairDone += AdjustHealthBarHeight;

        towerBehaviour.OnDamageTaken += StopRecoveryParticles;
        towerBehaviour.OnHealthFilled += StopRecoveryParticles;
        towerBehaviour.OnTowerRevived += PlayReviveParticles;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        towerBehaviour.OnRecoveryDone -= PlayRecoveryParticles;
        playerStats.OnWaveWon -= CheckIndicator;
        defencesBehaviourBase.OnRepairDone -= CheckIndicator;
        defencesBehaviourBase.OnRepairDone -= AdjustHealthBarHeight;


        towerBehaviour.OnDamageTaken -= StopRecoveryParticles;
        towerBehaviour.OnHealthFilled -= StopRecoveryParticles;
        towerBehaviour.OnTowerRevived -= PlayReviveParticles;

    }

    protected override void Start()
    {
        base.Start();
         AdjustHealthBarHeight();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void PlayRecoveryParticles()
    {
        for (int i = 0; i < recoveryParticles.Count; i++)
        {
            if (recoveryParticles[i].isPlaying)
            {
                return;
            }
            else
            {
                PlayParticles(recoveryParticles);
            }
        }
    }

    public void StopRecoveryParticles()
    {
        for (int i = 0; i < recoveryParticles.Count; i++)
        {
            if (recoveryParticles[i].isPlaying)
            {
                recoveryParticles[i].Stop();
            }
        }
    }

    private void PlayReviveParticles()
    {
        PlayParticle(reviveParticle);
    }

    protected override void CheckIndicator()
    {
        if (gameManager.allCities[playerStats.GetCityIndex()].gameObject != this.gameObject) return;

        base.CheckIndicator();

        if (!defencesBehaviourBase.GetIsRepariable())
        {
            indicatorAssigner.OnEnableIndicator(PointableTypes.home, Color.white);
        }
    }

    private void AdjustHealthBarHeight()
    {
        healthBar.transform.localPosition = new Vector3(healthBar.transform.localPosition.x, healthBarHeights[attackerDefenceStat.upgradeIndex],
            healthBar.transform.localPosition.z);
    }
}

