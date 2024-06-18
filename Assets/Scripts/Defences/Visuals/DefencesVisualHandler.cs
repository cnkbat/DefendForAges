using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DefencesVisualHandler : MonoBehaviour
{
    protected GameManager gameManager;
    protected DefencesBehaviourBase defencesBehaviourBase;
    protected PlayerStats playerStats;

    [Header("VFX")]
    [SerializeField] private List<ParticleSystem> destroyParticles = new List<ParticleSystem>();
    [SerializeField] private List<ParticleSystem> repairParticles = new List<ParticleSystem>();

    [Header("Indicator")]
    protected IndicatorAssigner indicatorAssigner;

    protected virtual void Awake()
    {
        defencesBehaviourBase = GetComponent<DefencesBehaviourBase>();
        indicatorAssigner = GetComponentInChildren<IndicatorAssigner>();
    }

    protected virtual void OnEnable()
    {
        gameManager = GameManager.instance;
        playerStats = PlayerStats.instance;

        playerStats.OnWaveWon += CheckIndicator;
        defencesBehaviourBase.OnRepairDone += CheckIndicator;

        defencesBehaviourBase.OnTargetDestroyed += PlayDestroyParticles;
        defencesBehaviourBase.OnTargetRevived += PlayRepairParticles;

    }

    protected virtual void OnDisable()
    {
        playerStats.OnWaveWon -= CheckIndicator;
        defencesBehaviourBase.OnRepairDone -= CheckIndicator;

        defencesBehaviourBase.OnTargetDestroyed -= PlayDestroyParticles;
        defencesBehaviourBase.OnTargetRevived -= PlayRepairParticles;
    }

    #region VFX
    public void PlayParticles(List<ParticleSystem> particles)
    {
        if (particles.Count == 0)
        {
            Debug.LogWarning(name + " Particles null");
            return;
        }

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }

    public void PlayDestroyParticles()
    {
        PlayParticles(destroyParticles);
    }

    public void PlayRepairParticles()
    {
        PlayParticles(repairParticles);
    }

    #endregion


    protected virtual void CheckIndicator()
    {
        if (defencesBehaviourBase.GetIsRepariable())
        {
            indicatorAssigner.OnEnableIndicator?.Invoke(PointableTypes.repair, Color.yellow);
        }
        else
        {
            indicatorAssigner.OnDisableIndicator?.Invoke();
        }
    }

}
