using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerVisualHandler : AttackerDefencesVisualHandler
{
    TowerBehaviour towerBehaviour;

    [Header("Recovery")]
    [SerializeField] private List<ParticleSystem> recoveryParticles = new List<ParticleSystem>();

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
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        towerBehaviour.OnRecoveryDone -= PlayRecoveryParticles;
        playerStats.OnWaveWon -= CheckIndicator;
        defencesBehaviourBase.OnRepairDone -= CheckIndicator;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void PlayRecoveryParticles()
    {
        PlayParticles(recoveryParticles);
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
