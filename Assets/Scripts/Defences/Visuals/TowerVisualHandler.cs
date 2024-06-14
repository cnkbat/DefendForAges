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
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        towerBehaviour.OnRecoveryDone -= PlayRecoveryParticles;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void PlayRecoveryParticles()
    {
        PlayParticles(recoveryParticles);
    }
}
