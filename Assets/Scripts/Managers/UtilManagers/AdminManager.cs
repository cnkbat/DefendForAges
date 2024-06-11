using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminManager : MonoBehaviour
{
    SaveManager saveManager;
    PlayerStats playerStats;
    [SerializeField] bool isAdmin;
    [SerializeField] GameObject adminPanel;
    [SerializeField] Button playerKillTestButton;

    void Start()
    {
        saveManager = SaveManager.instance;
        playerStats = PlayerStats.instance;
        playerKillTestButton.onClick.AddListener(() => playerStats.playerDeathHandler.Kill());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAdmin) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EnableDisableAdminPanel();
        }

        if (!adminPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGameData();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            GiveMoneyAndMeat();
        }

    }

    private void GiveMoneyAndMeat()
    {
        playerStats.IncrementMoney(1000);
        playerStats.IncrementMeat(1000);
        playerStats.IncrementXP(100);
    }

    private void EnableDisableAdminPanel()
    {
        adminPanel.SetActive(!adminPanel.activeSelf);
    }

    private void ResetGameData()
    {
        Time.timeScale = 0;

        saveManager.OnResetData?.Invoke();
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
