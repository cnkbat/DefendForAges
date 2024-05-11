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
    }

    private void OnDisable()
    {
        playerStats.OnExperiencePointChange += CheckLevelling;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LevelUp();
        }
    }
    public void CheckLevelling()
    {
        if (playerStats.experiencePoint >= playerSO.GetLevellingExperiencePoints()[playerStats.playerLevel])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        playerStats.playerLevel++;
        // ui update alttaki invoke a bağlanacak.
        OnLevelUp?.Invoke();
    }
}
