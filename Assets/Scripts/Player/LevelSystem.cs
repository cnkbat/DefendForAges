using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    RPGSystemSO playerSO;
    PlayerStats playerStats;
    public Action OnLevelUp;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerSO = playerStats.GetPlayerSO();
    }

    private void OnEnable()
    {
        playerStats.OnExperiencePointChange += CheckLevelling;
        OnLevelUp += playerStats.LevelUpPlayer;
    }

    private void OnDisable()
    {
        playerStats.OnExperiencePointChange -= CheckLevelling;
        OnLevelUp -= playerStats.LevelUpPlayer;
    }

    public void CheckLevelling()
    {
        if (playerStats.experiencePoint >= playerSO.GetLevellingExperiencePoints()[playerStats.GetPlayerLevel()])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        // ui update alttaki invoke a bağlanacak.
        OnLevelUp?.Invoke();
    }
}
