using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DefencesVisualHandler : MonoBehaviour
{
    protected GameManager gameManager;
    protected DefencesBehaviourBase defencesBehaviourBase;

    [Header("VFX")]
    [SerializeField] private List<ParticleSystem> destroyParticles = new List<ParticleSystem>();

    protected virtual void Awake()
    {
        defencesBehaviourBase = GetComponent<DefencesBehaviourBase>();
    }

    protected virtual void OnEnable()
    {
        gameManager = GameManager.instance;

        defencesBehaviourBase.OnTargetDestroyed += PlayDestroyParticles;
    }

    protected virtual void OnDisable()
    {
        defencesBehaviourBase.OnTargetDestroyed -= PlayDestroyParticles;
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

    #endregion
}
