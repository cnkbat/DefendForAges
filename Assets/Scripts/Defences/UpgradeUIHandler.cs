using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIHandler : MonoBehaviour
{
    UIManager uiManager;
    GameManager gameManager;

    void Start()
    {
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isAttackPhase) return;

        if (other.TryGetComponent(out PlayerCollisionHandler playerCollisionHandler))
        {
            EnableUpgradeUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCollisionHandler playerCollisionHandler))
        {
            DisableUpgradeUI();
        }
    }

    private void EnableUpgradeUI()
    {
        uiManager.EnableUpgradingButton();
    }

    private void DisableUpgradeUI()
    {
        uiManager.DisableUpgradingButton();
    }

}
