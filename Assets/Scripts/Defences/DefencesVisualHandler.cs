using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DefencesVisualHandler : MonoBehaviour
{
    GameManager gameManager;
    AttackerDefenceBehaviour attackerDefenceBehaviour;
    AttackerDefenceStat attackerDefenceStat;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

    [Header("VFX")]
    [SerializeField] private List<ParticleSystem> destroyParticles = new List<ParticleSystem>();

    private void Awake()
    {
        attackerDefenceBehaviour = GetComponent<TowerBehaviour>();
        attackerDefenceStat = GetComponent<AttackerDefenceStat>();
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;

        attackerDefenceBehaviour.OnTargetDestroyed += PlayDestroyParticles;
        attackerDefenceBehaviour.OnDamageTaken += UpdateHealthBarValue;
    }

    private void OnDisable()
    {
        attackerDefenceBehaviour.OnTargetDestroyed -= PlayDestroyParticles;
        attackerDefenceBehaviour.OnDamageTaken -= UpdateHealthBarValue;
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
        if (attackerDefenceBehaviour.GetIsDestroyed()) return;

        healthBar.gameObject.SetActive(true);
        healthBar.value = attackerDefenceBehaviour.GetCurrentHealth() / attackerDefenceStat.GetMaxHealth();

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
