using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental;
using TMPro;

public class LoadableBase : MonoBehaviour
{
    GameManager gameManager;
    SaveManager saveManager;
    PlayerStats playerStats;
    protected ObjectPooler objectPooler;
    public int currentCostLeftForUpgrade;

    [Header("State")]
    protected bool isFull;
    [SerializeField] private bool isRepairNeeded;

    [Header("Timers")]
    private float maxRepairTimer;
    private float currentRepairTimer;
    [SerializeField] private float coinAnimationBuffer = 0.075f;

    [Header("Texts")]
    [SerializeField] private TMP_Text currentCostLeftForUpgradeText;
    [SerializeField] private TMP_Text repairText;

    [Header("Events")]
    public Action OnLoadableFilled;
    public Action OnRepairDone;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.localPosition;
    }
    private void OnEnable()
    {
        gameManager = GameManager.instance;

        gameManager.OnWaveStarted += DisableObject;
    }

    private void OnDisable()
    {
        gameManager.OnWaveStarted -= DisableObject;
    }

    protected void Start()
    {
        objectPooler = ObjectPooler.instance;
        playerStats = PlayerStats.instance;
        saveManager = SaveManager.instance;

        maxRepairTimer = gameManager.repairTimer;
    }

    private void Update()
    {
        transform.localPosition = startPos;
    }

    public void Load()
    {

        if (isFull) return;

        if (isRepairNeeded)
        {
            currentRepairTimer -= Time.deltaTime;

            if (currentRepairTimer <= 0)
            {
                Repair();
            }

            return;
        }


        if (currentCostLeftForUpgrade > 0)
        {
            /*  if (playerStats.DecrementMoney(10) && currentCostLeftForUpgrade >= 10)
              {
                  currentCostLeftForUpgrade -= 10;

                  StartCoroutine(PlayCoinSpentAnimMultiple(10));

                  UpdateMoneyTextAndSave();
              } */
            if (playerStats.DecrementMoney(1))
            {
                currentCostLeftForUpgrade -= 1;

                PlayCoinSpentAnim();
                UpdateMoneyTextAndSave();
            }
        }
        CheckIfFulled();
    }

    private void UpdateMoneyTextAndSave()
    {
        UpdateCurrentMoneyText(currentCostLeftForUpgrade.ToString());
        saveManager.OnSaved?.Invoke();
    }

    private void PlayCoinSpentAnim()
    {
        GameObject spawnedObject = objectPooler.SpawnFromPool("Coin", playerStats.transform.position);

        spawnedObject.GetComponent<CurrencyAnimationHandler>().PlaySpendAnim();
        spawnedObject.transform.DOJump(transform.position, 3, 1, 1f)
            .OnComplete(() => spawnedObject.SetActive(false));
    }
    IEnumerator PlayCoinSpentAnimMultiple(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            PlayCoinSpentAnim();
            yield return new WaitForSeconds(coinAnimationBuffer);
        }
    }

    #region Repair Related
    private void Repair()
    {
        Debug.Log("repair");

        OnRepairDone?.Invoke();
        // dur simdi bakcaz 
    }

    public void ResetRepairTimer()
    {
        currentRepairTimer = maxRepairTimer;
    }

    #endregion

    #region State Related

    protected void CheckIfFulled()
    {
        if (currentCostLeftForUpgrade <= 0)
        {
            isFull = true;
            OnLoadableFilled?.Invoke();
        }
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Visuals

    private void UpdateCurrentMoneyText(string text)
    {
        currentCostLeftForUpgradeText.text = text;
    }

    #endregion

    #region Getters & Setters

    public void SetCurrentCostLeftForUpgrade(int value)
    {

        currentCostLeftForUpgrade = value;
        UpdateCurrentMoneyText(currentCostLeftForUpgrade.ToString());

        if (currentCostLeftForUpgrade > 0)
        {
            isFull = false;
        }
        else
        {
            isFull = true;
        }

    }


    public bool GetIsRepairNeeded()
    {
        return isRepairNeeded;
    }

    public void SetIsRepairNeeded(bool newBool)
    {
        currentCostLeftForUpgradeText.gameObject.SetActive(!newBool);
        repairText.gameObject.SetActive(newBool);

        if (newBool)
        {
            isRepairNeeded = newBool;
        }

    }

    #endregion

}

