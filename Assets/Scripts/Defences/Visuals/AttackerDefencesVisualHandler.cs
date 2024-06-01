using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackerDefencesVisualHandler : DefencesVisualHandler
{

    private AttackerDefenceBehaviour attackerDefenceBehaviour;
    private AttackerDefenceStat attackerDefenceStat;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    private float currentHealthBarDisappearTimer;

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
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        attackerDefenceBehaviour.OnDamageTaken -= UpdateHealthBarValue;
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
}
