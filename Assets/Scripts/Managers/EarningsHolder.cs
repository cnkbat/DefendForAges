using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarningsHolder : Singleton<EarningsHolder>
{
    PlayerStats playerStats;

    private float tempMeat;
    private float tempXP;
    private float tempCoin;

    public Action<float, float, float> OnTempEarningsUpdated;
    public Action<float> OnBonusMultiplierApplied;

    public Action<int, int, int> OnEarningsApply;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;

        OnEarningsApply += playerStats.EarnBonusAtWaveEnd;

        OnTempEarningsUpdated += EarnOnKill;

        OnBonusMultiplierApplied += ApplyEarningToPlayer;
    }

    private void OnDisable()
    {

        OnEarningsApply -= playerStats.EarnBonusAtWaveEnd;

        OnTempEarningsUpdated -= EarnOnKill;

        OnBonusMultiplierApplied -= ApplyEarningToPlayer;

    }

    public void EarnOnKill(float moneyValue, float xpValue, float meatValue)
    {
        IncrementTempMoney(moneyValue);
        IncrementTempXP(xpValue);
        IncrementTempMeat(meatValue);
    }

    #region Increment Values
    public void IncrementTempMeat(float value)
    {
        tempMeat += value;
    }
    public void IncrementTempMoney(float value)
    {
        tempCoin += value;
    }
    public void IncrementTempXP(float value)
    {
        tempXP += value;
    }
    #endregion

    public void ApplyEarningToPlayer(float multiplier)
    {
        OnEarningsApply?.Invoke((int)Math.Ceiling(tempMeat) * (int)multiplier,
            (int)Math.Ceiling(tempCoin) * (int)multiplier, (int)Math.Ceiling(tempXP) * (int)multiplier);
        ResetEarnings();
    }

    public void ResetEarnings()
    {
        tempMeat = 0;
        tempCoin = 0;
        tempXP = 0;
    }

}
