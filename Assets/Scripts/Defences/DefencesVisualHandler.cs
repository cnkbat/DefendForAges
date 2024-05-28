using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesVisualHandler : MonoBehaviour
{
    DefencesBehaviourBase defencesBehaviourBase;
    [SerializeField] private List<ParticleSystem> destroyParticles = new List<ParticleSystem>();

    private void Awake()
    {
        defencesBehaviourBase = GetComponent<DefencesBehaviourBase>();
    }

    private void OnEnable()
    {
        defencesBehaviourBase.OnTargetDestroyed += PlayDestroyParticles;
    }

    private void OnDisable()
    {
        defencesBehaviourBase.OnTargetDestroyed -= PlayDestroyParticles;
    }

    #region VFX
    public void PlayParticles(List<ParticleSystem> particles)
    {
        if(particles.Count == 0)
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
