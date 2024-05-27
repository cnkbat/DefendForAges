using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarningsHolder : Singleton<EarningsHolder>
{
    PlayerStats playerStats;

    private int earnedMeat;
    private int earnedXP;
    private int earnedMoney;

    public Action<int, int, int> OnTempEarningsUpdated;
    public Action<float> OnBonusMultiplierApplied;

    public Action<int, int, int> OnEarningsApply;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;

        OnEarningsApply += playerStats.EarnBonusAtWaveEnd;

        OnTempEarningsUpdated += EarnOnKill;

        // rewarded ad ile ui managera bağlanacak
        OnBonusMultiplierApplied += ApplyEarningToPlayer;
    }

    private void OnDisable()
    {
        
        OnEarningsApply -= playerStats.EarnBonusAtWaveEnd;

        OnTempEarningsUpdated -= EarnOnKill;

        OnBonusMultiplierApplied -= ApplyEarningToPlayer;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnBonusMultiplierApplied?.Invoke(1);
        }
    }

    public void EarnOnKill(int moneyValue, int xpValue, int meatValue)
    {
        IncrementMoney(moneyValue);
        IncrementXP(xpValue);
        IncrementMeat(meatValue);
    }

    #region Increment Values
    public void IncrementMeat(int value)
    {
        earnedMeat += value;
    }
    public void IncrementMoney(int value)
    {
        earnedMoney += value;
    }
    public void IncrementXP(int value)
    {
        earnedXP += value;
    }
    #endregion

    public void ApplyEarningToPlayer(float multiplier)
    {
        OnEarningsApply?.Invoke(earnedMeat * (int)multiplier, earnedMoney * (int)multiplier, earnedXP * (int)multiplier);
        ResetEarnings();
    }

    public void ResetEarnings()
    {
        earnedMeat = 0;
        earnedMoney = 0;
        earnedXP = 0;
    }
}
