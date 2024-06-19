using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AttackerDefencesVisualHandler : DefencesVisualHandler
{

    private AttackerDefenceBehaviour attackerDefenceBehaviour;
    private AttackerDefenceStat attackerDefenceStat;

    [SerializeField] private List<ParticleSystem> upgradeParticles = new List<ParticleSystem>();

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

    [Header("Enabling Parts")]
    [Tooltip("Boş olan yerlere boş obje yerleştirilebilir.")][SerializeField] private List<GameObject> enablingObjects;

    protected override void Awake()
    {
        base.Awake();
        attackerDefenceBehaviour = GetComponent<AttackerDefenceBehaviour>();
        attackerDefenceStat = GetComponent<AttackerDefenceStat>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        attackerDefenceBehaviour.OnDamageTaken += UpdateHealthBarValue;
        attackerDefenceStat.OnUpgraded += PlayUpgradeParticles;
        attackerDefenceStat.OnUpgraded += EnableUpgradedObjects;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        attackerDefenceBehaviour.OnDamageTaken -= UpdateHealthBarValue;
        attackerDefenceStat.OnUpgraded -= PlayUpgradeParticles;
        attackerDefenceStat.OnUpgraded -= EnableUpgradedObjects;
    }

    private void Start()
    {
        EnableUpgradedObjects();
    }

    protected virtual void Update()
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

    public void PlayUpgradeParticles()
    {
        PlayParticles(upgradeParticles);
    }

    public void EnableUpgradedObjects()
    {
        if (enablingObjects.Count == 0) return;

        for (int i = 0; i < enablingObjects.Count; i++)
        {
            enablingObjects[i].SetActive(false);
        }

        if (enablingObjects.Count > attackerDefenceStat.upgradeIndex)
        {
            enablingObjects[attackerDefenceStat.upgradeIndex].SetActive(true);

            Vector3 originalScale = enablingObjects[attackerDefenceStat.upgradeIndex].transform.localScale;
            enablingObjects[attackerDefenceStat.upgradeIndex].transform.localScale = Vector3.zero;

            enablingObjects[attackerDefenceStat.upgradeIndex].transform.DOScale(originalScale, 0.8f).SetEase(Ease.OutElastic);
        }

    }
}
