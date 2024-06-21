using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerVisualHandler : AttackerDefencesVisualHandler
{
    TowerBehaviour towerBehaviour;

    [Header("Tower VFXs")]
    [SerializeField] private List<ParticleSystem> recoveryParticles = new List<ParticleSystem>();
    [SerializeField] private ParticleSystem reviveParticle;

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

        towerBehaviour.OnDamageTaken -= StopRecoveryParticles;
        towerBehaviour.OnHealthFilled -= StopRecoveryParticles;
        towerBehaviour.OnTowerRevived -= PlayReviveParticles;

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
        base.CheckIndicator();

        if (!defencesBehaviourBase.GetIsRepariable())
        {
            indicatorAssigner.OnEnableIndicator(PointableTypes.home, Color.white);
        }
    }

}

