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

    [SerializeField] private bool isRepairNeeded;

    [Header("Timers")]
    private float maxRepairTimer;
    private float currentRepairTimer;
    [SerializeField] private float coinAnimationBuffer = 0.075f;

    [Header("Texts")]
    [SerializeField] private TMP_Text currentCostLeftForUpgradeText;
    [SerializeField] private TMP_Text repairText;

    [Header("Texts")]
    [SerializeField] private Image coinImage;
    [SerializeField] private Image repairImage;

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
        
        ResetRepairTimer();
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

        if (isRepairNeeded)
        {
            currentRepairTimer -= Time.deltaTime;
            Debug.Log("isRepairNeeded");
            if (currentRepairTimer <= 0)
            {
                Repair();
            }

            return;
        }


        if (currentCostLeftForUpgrade > 0)
        {
            if (playerStats.DecrementMoney(5) && currentCostLeftForUpgrade >= 5)
            {
                currentCostLeftForUpgrade -= 5;

                StartCoroutine(PlayCoinSpentAnimMultiple(5));

                UpdateMoneyTextAndSave();
            }
            else if (playerStats.DecrementMoney(1))
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
            OnLoadableFilled?.Invoke();
        }
    }

    public void DisableObject()
    {
        // gameObject.SetActive(false);
        gameObject.transform.DOScale(0, 1f).SetEase(Ease.InElastic).OnComplete(() => gameObject.SetActive(false));
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

    }


    public bool GetIsRepairNeeded()
    {
        return isRepairNeeded;
    }

    public void SetIsRepairNeeded(bool newBool)
    {
        coinImage.gameObject.SetActive(!newBool);
        // currentCostLeftForUpgradeText.gameObject.SetActive(!newBool);
        // repairText.gameObject.SetActive(newBool);
        repairImage.gameObject.SetActive(newBool);

        if (newBool)
        {
            isRepairNeeded = newBool;
        }

    }

    #endregion

}

